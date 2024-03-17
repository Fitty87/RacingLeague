using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class PlayerNetwork : NetworkBehaviour
{
    [SerializeField] private Transform spawnedObjectPrefab;

    private Transform spawnedObjectTransform;

    private NetworkVariable<MyCustomData> randomNumber = new NetworkVariable<MyCustomData>(
        new MyCustomData
        {
            _int=56,
            _bool=true,
        }, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);



    public struct MyCustomData : INetworkSerializable
    {
        public int _int;
        public bool _bool;
        public FixedString128Bytes message;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref _int);
            serializer.SerializeValue(ref _bool);
            serializer.SerializeValue(ref message);
        }
    }

    public override void OnNetworkSpawn()
    {
        randomNumber.OnValueChanged += (MyCustomData previousValue, MyCustomData newValue) =>
        {
            Debug.Log(OwnerClientId + " - " + newValue._int + "; " + newValue._bool + "; " + newValue.message);
        };
    }

    void Update()
    {

        if (!IsOwner)
            return;

        if (Input.GetKeyDown(KeyCode.T))
        {
            spawnedObjectTransform = null;
            spawnedObjectTransform = Instantiate(spawnedObjectPrefab);
            spawnedObjectTransform.GetComponent<NetworkObject>().Spawn(true);

            //TestServerRpc(new ServerRpcParams());

            //randomNumber.Value = new MyCustomData { _int = 3, _bool = false, message = "Hi am I" };
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            //Destroy(spawnedObjectTransform.gameObject);

            spawnedObjectTransform.GetComponent<NetworkObject>().Despawn(true); //vom Network, aber am Server bleiben
        }



            Vector3 moveDir = new Vector3(0, 0, 0);

        if (Input.GetKey(KeyCode.W))
            moveDir.z += 1;

        if (Input.GetKey(KeyCode.S))
            moveDir.z -= 1;

        if (Input.GetKey(KeyCode.A))
            moveDir.x -= 1;

        if (Input.GetKey(KeyCode.D))
            moveDir.x += 1;

        float moveSpeed = 3f;

        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }

    //Clients können die Funktionen vom Server aufrufen
    [ServerRpc]
    private void TestServerRpc(ServerRpcParams serverRpcParams) 
    { 
        Debug.Log("TestServerRpc" + OwnerClientId + serverRpcParams.Receive.SenderClientId); 
    }

    //Server ruft die Funktion auf, die für alle Clients dann ausgeführt wird
    [ClientRpc]
    private void TestClientRpc()
    {
        Debug.Log("TestClientRpc" + OwnerClientId);
    }
}
