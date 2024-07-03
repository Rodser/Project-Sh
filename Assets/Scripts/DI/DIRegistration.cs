using UnityEngine;

namespace DI
{
    public class DIRegistration
    {
        public System.Func<DIContainer, object> Factory { get; set; }
        public bool IsSingleton { get; set; }
        public object Instance { get; set; }
    }
}