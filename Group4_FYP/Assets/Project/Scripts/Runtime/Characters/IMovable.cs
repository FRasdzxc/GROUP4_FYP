using UnityEngine;

namespace PathOfHero.Characters
{
	public interface IMovable
	{
		void Move(Vector2 direction, bool sprint = false);
	}
}
