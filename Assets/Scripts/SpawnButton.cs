using UnityEngine;

namespace DefaultNamespace
{
    public class SpawnButton: MonoBehaviour, IInteractable
    {
        public string InteractMessage => objectInteractMessage;
        
        [SerializeField] GameObject spawnPrefab;
        
        [SerializeField] string objectInteractMessage;


        void Spawn()
        {
            var spawnedObject = Instantiate(spawnPrefab, position:transform.position + Vector3.up, rotation: Quaternion.identity);
            
            var randomSize = Random.Range(0.1f, 1f);
            spawnedObject.transform.localScale = Vector3.one * randomSize;
            
            var  randomColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
            spawnedObject.GetComponent<MeshRenderer>().material.color = randomColor;
        }

        
        public void Interact(InteractionController interactionController)
        {
            Spawn();
        }
    }
}