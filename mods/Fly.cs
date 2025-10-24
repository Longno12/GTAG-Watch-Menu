using UnityEngine;

namespace Watch_Menu.mods
{
    internal class Fly
    {
        public static float speed = 10f;

        public static void Fly1()
        {
            if (ControllerInputPoller.instance.rightControllerIndexFloat < 1f) return;
            var player = GorillaLocomotion.GTPlayer.Instance;
            if (player == null) return;
            var head = player.headCollider;
            var body = player.transform;
            if (head == null || body == null) return;
            Vector3 forward = head.transform.forward * speed * Time.deltaTime;
            body.position += forward;
            var rb = player.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
            }
        }
    }
}
