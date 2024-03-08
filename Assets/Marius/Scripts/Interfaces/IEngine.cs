using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MariusNameSpace
{
    //An interface can't extend or inherit from Monobehaviour
    //Interfaces are there to say: "I'm gonna have this set of variables and this set of functions in a class that inherits from me"
    //By inheriting from this interface, we're gonna have these items in our class
    public interface IEngine 
    {
        void InitEngine();
        void UpdateEngine(Rigidbody rb, Drone_Inputs inputs);
    }
}
