using Shudder.Presenters;
using UnityEngine;

namespace Shudder.Gameplay.Views
{
    public class GroundView : MonoBehaviour
    {
        [SerializeField] private Transform _anchorPoint = null;
        [SerializeField] private GameObject _top = null;
        [SerializeField] private Color[] _colors;
        
        public GroundPresenter Presenter { get; private set; }
        public Transform AnchorPoint => _anchorPoint;

        public void Construct(GroundPresenter groundPresenter)
        {
            Presenter = groundPresenter;
            Presenter.SetView(this);
        }

        /*private async void SetColorAsync()
        {
            if(IsStationary())
                return;
            await UniTask.Delay(300);

            int index = (int) GroundType;
            _topMaterial.color = _colors[index];
        }
        
        private Material GetColor()
        {
            return IsStationary() ? null : _top.GetComponent<Renderer>().materials[0];
        }
        
        private bool IsStationary()
        {
            return GroundType == GroundType.Pit || GroundType == GroundType.Hole || GroundType == GroundType.Wall;
        }*/
    }
}