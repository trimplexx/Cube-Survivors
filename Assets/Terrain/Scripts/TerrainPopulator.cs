using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainPopulator : MonoBehaviour
{
    public Player_Movement player;
    public Chunk chunk_prefab;

    /*Słownik reprezentujący chunki wraz z ich współrzędnymi*/
    private Dictionary<Vector3Int, Chunk> chunks = new Dictionary<Vector3Int, Chunk>();
    
    /*Czas (w sekundach) pomiędzy czyszczeniem chunków*/
    private float cull_timer = 30f;
    private float time_since_last_cull = 0f;

    private void FixedUpdate()
    {
        CreateChunks();

        /*Aktualizuj licznik czasu od ostatniego czyszczenia chunków*/
        time_since_last_cull += Time.fixedDeltaTime;

        /*Jeśli upłynął wymagany czas, wykonaj czyszczenie chunków i zresetuj licznik czasu*/
        if (time_since_last_cull >= cull_timer)
        {
            CullChunks();
            time_since_last_cull = 0f;
        }
    }

    private void CreateChunks()
    {
        if (!ShouldCreateNewChunk()) return;

        /*Obliczanie współrzędnych chunka dla wektora INTÓW (Np przejście z chunka (0, 0, 0) -> (0, 0, 1))*/
        var player_cord = Vector3Int.RoundToInt(player.transform.position / chunk_prefab.size);

        /*Pętla tworząca chunki*/
        foreach (var pos in PositionsAround(player_cord))
        {
            CreateNewChunk(pos, chunk_prefab);
        }
    }

    private bool ShouldCreateNewChunk()
    {
        return true;
    }

    private Chunk CreateNewChunk(Vector3Int position, Chunk prefab)
    {
        if (chunks.ContainsKey(position)) return default; //Jeżeli chunki istnieją to nie rób nic
        var instance = Instantiate(prefab.gameObject);
        instance.transform.position = (Vector3) position * chunk_prefab.size; //Oblicza pozycję chunka
        Chunk chunkInstance = instance.GetComponent<Chunk>();
        chunks.Add(position, chunkInstance); //Dodaje chunki do słownika chunków
        return chunkInstance; //Zwraca utworzony chunk
    }

    private void CullChunks()
    {
        /*Obliczanie współrzędnych chunka dla wektora INTÓW (Np przejście z chunka (0, 0, 0) -> (0, 0, 1))*/
        var player_cord = Vector3Int.RoundToInt(player.transform.position / chunk_prefab.size);

        /*Lista przechowująca chunki do usunięcia*/
        List<Vector3Int> chunks_to_remove = new List<Vector3Int>();

        /*Pętla dodająca chunki do usunięcia*/
        foreach (var chunk_pos in chunks.Keys)
        {
            if (!IsChunkNearPlayer(chunk_pos, player_cord))
            {
                chunks_to_remove.Add(chunk_pos);
            }
        }
        /*Pętla niszcząca obiekty oraz usuwająca z listy zniszczone chunki*/
        foreach (var chunk_pos in chunks_to_remove)
        {
            Destroy(chunks[chunk_pos].gameObject);
            chunks.Remove(chunk_pos);
        }
    }

    private bool IsChunkNearPlayer(Vector3Int chunkPos, Vector3Int playerPos)
    {
        /*Obliczanie odległości chunków od pozycji gracza*/
        int distanceX = Mathf.Abs(chunkPos.x - playerPos.x);
        int distanceZ = Mathf.Abs(chunkPos.z - playerPos.z);

        /*Sprawdź czy chunk jest na tej samej pozycji co gracz*/
        return distanceX <= 1 && distanceZ <= 1;
    }

    /* Funkcja dodająca do współrzędnych pierwszych chunków pozycję gracza 
            w celu ustalenia współrzędnych dla nowych fragmentów           */
    private Vector3Int[] PositionsAround(Vector3Int position)
    {
        return new Vector3Int[]
        {
            position + new Vector3Int(1, 0, 0),
            position + new Vector3Int(0, 0, 1),
            position + new Vector3Int(1, 0, 1),
            position + new Vector3Int(-1, 0, 0),
            position + new Vector3Int(0, 0, -1),
            position + new Vector3Int(-1, 0, -1),
            position + new Vector3Int(-1, 0, 1),
            position + new Vector3Int(1, 0, -1)
        };
    }
}
