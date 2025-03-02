using UnityEngine;

public class InteractablesManager : MonoBehaviour
{
    private PrizeBlock[] _prizeBlocks;
    private BrickBlock[] _brickBlocks;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _prizeBlocks = GetComponentsInChildren<PrizeBlock>();
        _brickBlocks = GetComponentsInChildren<BrickBlock>();
    }

    public void ResetInteractables()
    {
        foreach (var prizeBlock in _prizeBlocks)
        {
            prizeBlock.Reset();
        }

        foreach (var brickBlock in _brickBlocks)
        {
            brickBlock.Reset();
        }
    }
}
