using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadraticInterpolation : MonoBehaviour
{
    public GameObject target;
    public Vector2 curve;
    public float speed = 1;
    public bool targetIsCanvas = true;
    public bool goToTarget = false;
    public bool destroyOnFinish = true;
    private Vector2 finalTarget;
 
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        if(this.GetComponent<Bonus>()._myType == BonusTypes.Types.LIGHT)
            target = GameObject.Find("LightLevel").gameObject;
        
        else if(this.GetComponent<Bonus>()._myType == BonusTypes.Types.VELOCITY)
            target = GameObject.Find("VelocityLevel").gameObject;

        else if(this.GetComponent<Bonus>()._myType == BonusTypes.Types.COORDINATION)
             target = GameObject.Find("CoordinationLevel").gameObject;

        else if(this.GetComponent<Bonus>()._myType == BonusTypes.Types.HIPOCAMPO)
             target = GameObject.Find("HipocampoLevel").gameObject;
    }

    

    public void Start()
    {
        finalTarget=target.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
      if(goToTarget){
          GoToTarget();
          goToTarget=false;
      }
    }
    public void GoToTarget() //lamar cuando el personaje toca el bonus
    {
        StopCoroutine("Quadratic");
        StartCoroutine("Quadratic");

    }
    IEnumerator Quadratic()
    {
        Vector2 crv = new Vector2(finalTarget.x + curve.x, finalTarget.y + curve.y);
    

        while (Mathf.Abs(this.transform.position.x - finalTarget.x) > 0.01f && Mathf.Abs(this.transform.position.y - finalTarget.y) > 0.01f)
        {
            crv = Vector2.Lerp(crv, finalTarget, speed * Time.deltaTime);
            transform.position = Vector2.Lerp(transform.position, crv, speed * Time.deltaTime);
            yield return null;
        }
        if(destroyOnFinish){
            Destroy(this.gameObject);
        }
        yield return null;
    }
}
