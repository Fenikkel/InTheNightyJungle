using UnityEngine;
using System.Collections;

//Interpolation between 2 points with a Bezier Curve (cubic spline)
public class BezierCurve : MonoBehaviour 
{
    //Has to be at least 4 so-called control points
    public Transform startPoint;
    public Transform endPoint;
    public Transform controlPointStart;
    public Transform controlPointEnd;

	public Transform movingObject;
	private bool direction;
	private int currentLoop;

	private float resolution;
	private int totalLoops;
	public float speed;

    //Easier to use ABCD for the positions of the points so they are the same as in the tutorial image
    Vector3 A, B, C, D;

	void Start()
	{
		direction = true;
		currentLoop = 1;

        resolution = 0.02f;
        totalLoops = Mathf.FloorToInt(1f / resolution);

		movingObject.position = DeCasteljausAlgorithm(1 * resolution);
	}

    //Display without having to press play
    void OnDrawGizmos()
    {
        A = startPoint.position;
        B = controlPointStart.position;
        C = controlPointEnd.position;
        D = endPoint.position;

	//The Bezier curve's color
        Gizmos.color = Color.white;

        //The start position of the line
        Vector3 lastPos = A;

        //The resolution of the line
        //Make sure the resolution is adding up to 1, so 0.3 will give a gap at the end, but 0.2 will work

        //How many loops?

        for (int i = 1; i <= totalLoops; i++)
        {
            //Which t position are we at?
            float t = i * resolution;

            //Find the coordinates between the control points with a Catmull-Rom spline
            Vector3 newPos = DeCasteljausAlgorithm(t);

            //Draw this line segment
            Gizmos.DrawLine(lastPos, newPos);

            //Save this pos so we can draw the next line segment
            lastPos = newPos;
        }
		
	//Also draw lines between the control points and endpoints
        Gizmos.color = Color.green;

        Gizmos.DrawLine(A, B);
        Gizmos.DrawLine(C, D);
    }

	void Update()
	{
		if(direction)
		{
			float t = currentLoop * resolution * speed;
			Vector3 newPos = DeCasteljausAlgorithm(t);
			movingObject.position = newPos;

			print("hola");

			currentLoop++;

			if(currentLoop == totalLoops)
			{
				direction = false;
			}
		}
		else
		{
			float t = currentLoop * resolution * speed;
			Vector3 newPos = DeCasteljausAlgorithm(t);
			movingObject.position = newPos;

			print("adios");

			currentLoop--;
			
			if(currentLoop == 1)
			{
				direction = true;
			}
		}
	}

    //The De Casteljau's Algorithm
    Vector3 DeCasteljausAlgorithm(float t)
    {
        //Linear interpolation = lerp = (1 - t) * A + t * B
        //Could use Vector3.Lerp(A, B, t)

        //To make it faster
        float oneMinusT = 1f - t;
        
        //Layer 1
        Vector3 Q = oneMinusT * A + t * B;
        Vector3 R = oneMinusT * B + t * C;
        Vector3 S = oneMinusT * C + t * D;

        //Layer 2
        Vector3 P = oneMinusT * Q + t * R;
        Vector3 T = oneMinusT * R + t * S;

        //Final interpolated position
        Vector3 U = oneMinusT * P + t * T;

        return U;
    }
}