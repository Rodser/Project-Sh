using Shudder.Presenters;
using UnityEngine;

namespace Shudder.Vews
{
    public class GroundView : MonoBehaviour
    {
        [SerializeField] private Transform _anchorPoint;
        
        public GroundPresenter Presenter { get; private set; }
        public Transform AnchorPoint => _anchorPoint;

        public void Construct(GroundPresenter groundPresenter)
        {
            Presenter = groundPresenter;
            Presenter.SetView(this);
        }
    }
}