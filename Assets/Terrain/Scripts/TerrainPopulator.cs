using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainPopulator : MonoBehaviour
{
    public Player_Movement player;
    public Chunk chunk_prefab;

    /*Słownik reprezentujący chunki wraz z ich współrzędnymi*/
    private Dictionary<Vector3Int, Chunk> chunks = new Dictionary<Vector3Int, Chunk>();

    private void FixedUpdate()
    {
        CreateChunks();
    }

    private void CreateChunks()
    {
        if (!ShouldCreateNewChunk()) return;

        /*Obliczanie współrzędnych chunka dla wektora INTÓW (Np przejście z chunka (0, 0, 0) -> (0, 0, 1))*/
        var player_position = player.transform.position / chunk_prefab.size;
        var player_cord = Vector3Int.RoundToInt(player_position);

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
        if (chunks.ContainsKey(position)) return default; //Jeżeli chunki istnieją to nie rób nic.
        var instance = Instantiate(prefab.gameObject);
        instance.transform.position = (Vector3) position * chunk_prefab.size; //Oblicza pozycję chunka
        Chunk chunkInstance = instance.GetComponent<Chunk>();
        chunks.Add(position, chunkInstance); //Dodaje chunki słownika chunków
        return chunkInstance; //Zwraca utworzony chunk
    }

    private void CullChunks()
    {

    }

    /*Tworzenie pierwszych segmentów mapy (9x9)*/
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
