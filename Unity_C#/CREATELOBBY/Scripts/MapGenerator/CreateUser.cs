using UnityEngine;

public class CreateUser : Photon.MonoBehaviour
{
    private PhotonView PhotonView;

    int[,] ships;

    private void Awake()
    {
        PhotonView = GetComponent<PhotonView>();

        ships = new int[10, 10];

        for (int j = 0; j < 10; j++)
        {
            for (int i = 0; i < 10; i++)
            {
                ships[i, j] = Random.Range(0f, 1f) < 0.5f ? 1 : 0;
            }
        }
    }

    private void Update()
    {
        if (PhotonView.isMine && Input.GetMouseButtonDown(0))
        {
            Vector2 v2 = Input.mousePosition;
            v2 = Camera.main.ScreenToWorldPoint(v2);
            photonView.RPC("ChangeColor", PhotonTargets.All, ((int)v2.x - 1), ((int)v2.y));
        }
    }

    [PunRPC]
    private void ChangeColor(int x, int y)
    {
        // если это не моё игровое поле и корабль находится в указанных координатах
        // то вызывается метод обозначения попадания/промаха
        print(x + " " + y);
        if (!PhotonView.isMine)        
        {
            if (ShipChecker(x, y))
            {
                MainMap field = FindObjectOfType<MainMap>();
                field.MainFieldUpdater(x, y, true);
                photonView.RPC("TargetHitting", PhotonTargets.Others, x, y, true);     // при попадании метод выполняется на противоположном устройстве
            }
            else
            {
                MainMap field = FindObjectOfType<MainMap>();
                field.MainFieldUpdater(x, y, false);
                photonView.RPC("TargetHitting", PhotonTargets.Others, x, y, false);
            }
        }
    }

    private bool ShipChecker(int x, int y)
    {
        print("HIT " + ships[x, y]);

        if (ships[x, y] == 1)
        {
            return true;
        }

        return false;   
    }
    [PunRPC]
    private void TargetHitting(int x, int y, bool target_hit)
    {
        MapGenerator field = FindObjectOfType<MapGenerator>();
        field.GameFieldUpdater(x, y, target_hit);
    }

    /*
    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo msg)  // чтение и запись в поток
    {
        Vector3 pos = transform.position;
        Quaternion rotation = transform.rotation;

        // Записываем нашу позицию в поток
        // метод 'Serialize' - когда этот метод вызван с потоком на запись (stream.isWriting)
        // мы записываем в наш поток значения переменных, а когда метод вызван с потоком на чтение (stream.isReading)
        // наоборот - читаем из потока в переменные переданные по ссылке 'pos' и 'rotation'
        stream.Serialize(ref pos);
        stream.Serialize(ref rotation);


        if (stream.isReading)
        {
            oldPos = transform.position;
            oldRot = transform.rotation;
            newPos = pos;
            newRot = rotation;
            offsetTime = 0;
            isSynh = true;
        }
    }*/
}
