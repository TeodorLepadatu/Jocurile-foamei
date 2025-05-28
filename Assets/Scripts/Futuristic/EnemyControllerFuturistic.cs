using UnityEngine;

public class EnemyControllerFuturistic : MonoBehaviour
{

		private Animator anim;
		public bool isDead = false;

		void Start()
		{
			anim = GetComponent<Animator>();
		}

		public void Die()
		{
			if (isDead) return;
			isDead = true;

			anim.SetBool("isDead", true);

			Destroy(gameObject, 5f);
		}



}
