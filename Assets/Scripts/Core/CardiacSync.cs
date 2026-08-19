using CardioCore;
using UnityEngine;

namespace Core
{
    public sealed class CardiacSync : MonoBehaviour
    {
        [SerializeField] private DataClient client;
        [SerializeField] private CardiacCycleController cycle;

        private void OnEnable() { if (client != null) client.OnTwinState += Push; }
        private void OnDisable() { if (client != null) client.OnTwinState -= Push; }

        private void Push(TwinState s) => cycle.Sync(s.Bpm, s.Phase, s.RS1Ms, s.S1S2Ms);
    }
}