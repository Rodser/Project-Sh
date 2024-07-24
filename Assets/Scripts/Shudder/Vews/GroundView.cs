using Shudder.Presenters;
using UnityEngine;

namespace Shudder.Vews
{
    public class GroundView : MonoBehaviour
    {
        [SerializeField] private Transform _anchorPoint;
        [SerializeField] private GameObject _top = null;
        [SerializeField] private Color[] _colors;
        
        public GroundPresenter Presenter { get; private set; }
        public Transform AnchorPoint => _anchorPoint;

        public void Construct(GroundPresenter groundPresenter)
        {
            Presenter = groundPresenter;
            Presenter.SetView(this);
        }
    }
}