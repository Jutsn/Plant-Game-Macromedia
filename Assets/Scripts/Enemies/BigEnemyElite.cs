using UnityEngine;

public class BigEnemyElite : BigEnemy
{
	protected override void DropResources()
	{
		
		int dropAmount = GetDropAmount();
		
		for (int i= 0; i < dropAmount; i++)
		{
			GameObject resource2 = Resource2Pool.Instance.GetPooledObject();

			if (resource2 != null)
			{
				resource2.transform.position = transform.position;
				resource2.transform.rotation = transform.rotation;
				resource2.SetActive(true);
			}
		}
	}

	protected virtual int GetDropAmount()
	{
		int dropAmount = Random.Range(GameManager.Instance.GetMinDropAmountResource2Elite(), GameManager.Instance.GetMaxDropAmountResource2Elite());
		return dropAmount;
	}
}
