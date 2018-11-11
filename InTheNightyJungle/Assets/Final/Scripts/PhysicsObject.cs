using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsObject : MonoBehaviour {

    public float minGroundNormalY = .65f;
    public float gravityModifier = 1f;

    protected Vector2 targetVelocity;
    protected bool grounded;
    protected Vector2 groundNormal;
    protected Rigidbody2D rb2d;
    protected Vector2 velocity;
    protected ContactFilter2D contactFilter;
    protected RaycastHit2D[] hitBuffer = new RaycastHit2D[16];
    protected List<RaycastHit2D> hitBufferList = new List<RaycastHit2D>(16);


    protected const float minMoveDistance = 0.001f; //Distancia mínima de movimiento para empezar a comprobar colisiones con el objeto
    protected const float shellRadius = 0.01f; //El shell radius es una distancia que se añadirá a la distancia para realizar el cast de las colisiones, de manera que así que se evita quedarse enganchado dentro de un collider

    void OnEnable()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        initialization();
    }

    protected virtual void initialization()
    {
        ContactFilterInitialization();
    }

    protected void ContactFilterInitialization()
    {
        contactFilter.useTriggers = false; //No tomará los colliders triggereados
        contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer)); //Con esto le estamos diciendo que el contactFilter guarde como máscara de layers aquellas layers contra las que puede colisionar el objeto que estamos tratando, que pueden ser definidas cambiando los settings de las físicas 2D de la escena. Una maravilla, la verdad :)
        print(contactFilter + " " + gameObject.name);
        contactFilter.useLayerMask = true;
    }

    void Update()
    {
        targetVelocity = Vector2.zero;
        ComputeVelocity();
    }

    protected virtual void ComputeVelocity()
    {

    }

    void FixedUpdate()
    {
        velocity += gravityModifier * Physics2D.gravity * Time.deltaTime; //Calculamos la velocidad del objeto físico en función de cómo se ve afectado por la gravedad desde el último frame
        velocity.x = targetVelocity.x;

        grounded = false;

        Vector2 deltaPosition = velocity * Time.deltaTime; //Teniendo en cuenta la velocidad total teórica del objeto, calculamos la distancia teórica a recorrer por el objeto en este frame

        Vector2 moveAlongGround = new Vector2(groundNormal.y, -groundNormal.x); //Calculamos el movimiento sobre una plataforma teniendo en cuenta la inclinación de la misma, de manera que la dirección del movimiento seguirá esa inclinación

        Vector2 move = moveAlongGround * deltaPosition.x;

        Movement(move, false);

        move = Vector2.up * deltaPosition.y; //Nos quedamos con la componente Y de la distancia teórica

        Movement(move, true);
    }

    void Movement(Vector2 move, bool yMovement)
    {
        float distance = move.magnitude;

        if (distance > minMoveDistance) //Si la distancia a recorrer es mayor que la mínima, empezamos a comprobar cosas
        {
            int count = rb2d.Cast(move, contactFilter, hitBuffer, distance + shellRadius); //Cuenta y guarda los colliders (solo los pasan el contactFilter) con los que va a colisionar el objeto en dirección asignada (move), en la distancia dada (distance + shellRadius)
            hitBufferList.Clear();
            for (int i = 0; i < count; i++)
            {
                hitBufferList.Add(hitBuffer[i]);
            }

            for (int i = 0; i < hitBufferList.Count; i++)
            {
                Vector2 currentNormal = hitBufferList[i].normal;
                if (currentNormal.y > minGroundNormalY) //Esta mierda arregla el que un personaje se quede suspendido en el aire cuando colisiona con una plataforma y mantienes el input activo!!!
                {
                    grounded = true;
                    if (yMovement) //Solo se entra en este condicional cuando se trata del movimiento en Y (caer)
                    {
                        groundNormal = currentNormal; //Guardamos la normal de la plataforma sobre la que está colocado el personaje
                        currentNormal.x = 0;
                    }
                }

                float projection = Vector2.Dot(velocity, currentNormal); //Se resta velocidad menos la normal de la collider actual
                if (projection < 0)
                {
                    velocity = velocity - projection * currentNormal; //Se contrarresta la velocidad si la velocidad supera a la normal
                }

                float modifiedDistance = hitBufferList[i].distance - shellRadius;
                distance = modifiedDistance < distance ? modifiedDistance : distance;
            }


        }

        rb2d.position = rb2d.position + move.normalized * distance; //Cambiamos la posición del RigidBody añadiendo el movimiento teórico calculado previamente
    }
}
