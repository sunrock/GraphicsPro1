using UnityEngine;
using System.Collections;

public class TerrainGen : MonoBehaviour {

	private Terrain terrain;
	private int hmapResolution;
	private float[,] heightMap;

	public GameObject wtpl;


	// Use this for initialization
	void Start () {
		
		terrain = this.GetComponent<Terrain>();

		TerrainData terrData = terrain.terrainData;

		hmapResolution = terrData.heightmapResolution;

//		Debug.Log ("heightmapResolution: " + hmapResolution);

		int recurRsln = hmapResolution - 1;

		heightMap = new float[hmapResolution, hmapResolution];

		if (isPow2 (recurRsln)) 
		{
			Debug.Log ("recurRsln: " + recurRsln);
			heightMap = DiamondSquare (recurRsln);
		}

		terrData.SetHeights(0, 0, heightMap);


//		Debug.Log ("waterLevel x: " + wtpl.transform.position.x);
//		Debug.Log ("waterLevel y: " + wtpl.transform.position.y);
//		Debug.Log ("waterLevel z: " + wtpl.transform.position.z);


		float waterLevel = terrData.GetHeight(0,0);
		Debug.Log ("waterLevel: " + waterLevel);



		wtpl.transform.position = new Vector3 (0, waterLevel, 0);

//		Debug.Log ("waterLevel x: " + wtpl.transform.position.x);
//		Debug.Log ("waterLevel y: " + wtpl.transform.position.y);
//		Debug.Log ("waterLevel z: " + wtpl.transform.position.z);

	
	}
	
	// Update is called once per frame
	void Update () {

	
	}

	// Returns true if a is a power of 2, else false
	private bool isPow2 (int recur)
	{
		return (recur & (recur - 1)) == 0;
	}

	private float randomValue(float m)
	{
		float margin = m;

		if (margin > 0.5f) 
		{
			margin = 1.0f - m;
		} 

		float rand = Random.Range (-margin, margin);

		return rand;

	}

	private float[,] DiamondSquare(int size){
		
		float minHeight = 0.0f;
		float maxHeight = 1.0f;

		float[,] initHmap = new float [size + 1, size + 1];

		float initRandom = Random.Range(minHeight, maxHeight);
		initHmap [0, 0] = initRandom;//Random.Range(minHeight, maxHeight);
		initHmap[size,0] = initRandom;// Random.Range(minHeight, maxHeight);
		initHmap[0,size] = initRandom;//Random.Range(minHeight, maxHeight);
		initHmap[size,size] = initRandom;//Random.Range(minHeight, maxHeight);

		/*
		 * Generate matrix, calculate values in all 
		 * 
		 *    sq0 -- dm0 -- sq1
		 *     |      |      |
		 *     |      |      |
		 *    dm1 -- ct  -- dm2
		 *     |      |      |
		 * 	   |      |      |
		 *    sq2 -- dm3 -- sq3
		 *
		 */

		float sq0, sq1, sq2, sq3;
		float dm0, dm1, dm2, dm3;
		float ct;

//		float margin = 1.0f;
		// Roughness of the map. Decreases as the algorithm creates the map.
		float roughness = 0.6f;

		for (int i = size; i > 1; i /= 2)
		{
			
			// do diamond step
			for (int y = 0; y < size; y += i)
			{
				for (int x = 0; x < size; x += i)
				{
					// four square nodes
					sq0 = initHmap [x, y];

					sq1 = initHmap [x + i, y];
					sq2 = initHmap [x, y + i];
					sq3 = initHmap [x + i, y + i];

					// centre nodes
					ct = ((sq0 + sq1 + sq2 + sq3) / 4.0f);
//					margin = marginRange (ct);

					initHmap[x+i/2, y+i/2] = ct + randomValue(ct) * roughness;
				}
			}

			// do square step
			for (int y = 0; y < size; y += i)
			{
				for (int x = 0; x < size; x += i)
				{
					sq0 = initHmap [x, y];
					sq1 = initHmap [x + i, y];
					sq2 = initHmap [x, y + i];
					sq3 = initHmap [x + i, y + i];
					ct =  initHmap [x + i / 2, y + i / 2];

					dm0 = (y <= 0) ? (sq0 + sq1 + ct) / 3.0f : (sq0 + sq1 + ct + initHmap[x + i / 2, y - i / 2]) / 4.0f;
					dm1 = (x <= 0) ? (sq0 + ct + sq2) / 3.0f : (sq0 + ct + sq2 + initHmap[x - i / 2, y + i / 2]) / 4.0f;
					dm2 = (x >= size - i) ? (sq1 + ct + sq3) / 3.0f :
						(sq1 + ct + sq3 + initHmap[x + i + i / 2, y + i / 2]) / 4.0f;
					dm3 = (y >= size - i) ? (ct + sq2 + sq3) / 3.0f :
						(ct + sq2 + sq3 + initHmap[x + i / 2, y + i + i / 2]) / 4.0f;

					initHmap [x + i / 2, y] = dm0 + randomValue (dm0) * roughness;
					initHmap [x, y + i / 2] = dm1 + randomValue (dm1) * roughness;
					initHmap [x + i, y + i / 2] = dm2 + randomValue (dm2) * roughness;
					initHmap [x + i / 2, y + i] = dm3 + randomValue (dm3) * roughness;
				}
			}

			roughness /= 2.0f;
		}

		return initHmap;

	}
}
