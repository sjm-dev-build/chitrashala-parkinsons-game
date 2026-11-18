using UnityEngine;
using Leap;

public class FingertipFollower : MonoBehaviour
{
    LeapServiceProvider provider;

    void Update()
    {
        if (provider == null) provider = FindFirstObjectByType<LeapServiceProvider>();
        if (provider == null) return;

        Frame frame = provider.CurrentFrame;
        if (frame == null || frame.Hands.Count == 0) return;

        Hand hand = frame.Hands[0];
        Finger index = hand.fingers[1]; // 0 = thumb, 1 = index
        transform.position = index.TipPosition;
    }
}