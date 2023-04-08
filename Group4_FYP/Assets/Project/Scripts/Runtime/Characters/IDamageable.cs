namespace PathOfHero.Characters
{
	public interface IDamageable
    {
		int Health { get; }
		int MaxHealth { get; }

		void TakeDamage(int amount);
		void Heal(int amount);
		void Death();
	}
}
