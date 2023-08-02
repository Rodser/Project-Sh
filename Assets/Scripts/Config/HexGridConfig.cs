using Rodser.Model;
using UnityEngine;

namespace Rodser.Config
{
    [CreateAssetMenu(fileName = "HexGrid", menuName = "Game/HexGrid", order = 7)]
    public class HexGridConfig : ScriptableObject
    {
        [field: SerializeField] public Ground Prefab { get; private set; } = null;
        [field: SerializeField] public int Width { get; private set; } = 5;
        [field: SerializeField] public int Height { get; private set; } = 10;
        [field: SerializeField] public float SpaceBetweenCells { get; private set; } = 0.86f;
        [field: SerializeField] public float HeightCell { get; private set; } = 1f;
    }
}
