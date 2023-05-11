using UnityEngine;

public class EnemyCounter : MonoBehaviour
{
	public static int count { get; private set; } = 0;

	private void Awake() => count++;
	private void OnDestroy() => count--;
}
