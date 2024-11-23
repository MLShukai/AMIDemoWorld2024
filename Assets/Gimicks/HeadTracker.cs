
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace VConf2024
{
    public class HeadTracker : UdonSharpBehaviour
    {
        [SerializeField]
        private string[] _targetPlayerDisplayNameList;
        private VRCPlayerApi _targetPlayer = null;

        private void Start()
        {
            TrySetTargetPlayer(Networking.LocalPlayer);
        }

        private void Update()
        {
            if (_targetPlayer != null)
            {
                var headPosition = _targetPlayer.GetBonePosition(HumanBodyBones.Head);
                var headRotation = _targetPlayer.GetBoneRotation(HumanBodyBones.Head);
                this.transform.position = headPosition;
                this.transform.rotation = headRotation;
            }
        }

        public override void OnPlayerJoined(VRCPlayerApi player)
        {
            TrySetTargetPlayer(player);
        }

        public override void OnPlayerLeft(VRCPlayerApi player)
        {
            if (_targetPlayer == player)
            {
                _targetPlayer = null;
            }
            var allPlayers = new VRCPlayerApi[50];
            VRCPlayerApi.GetPlayers(allPlayers);
            foreach (var existPlayer in allPlayers)
            {
                if (existPlayer == null)
                {
                    continue;
                }
                if (TrySetTargetPlayer(existPlayer))
                {
                    break;
                }
            }
        }

        private bool TrySetTargetPlayer(VRCPlayerApi player)
        {
            foreach (var targetPlayerDisplayName in _targetPlayerDisplayNameList)
            {
                if (player.displayName == targetPlayerDisplayName)
                {
                    _targetPlayer = player;
                    return true;
                }
            }
            return false;
        }
    }
}
