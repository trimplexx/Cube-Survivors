using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class TerrainPopulator : MonoBehaviour
{
    public Player_Movement player;
    public Chunk chunk_prefab;
    
    /*Lista prefabów (obiektów) umieszczanych na chunku*/
    public List<BiomeSettings> biome_settings = new List<BiomeSettings>();

    /*Czas (w sekundach) pomiędzy czyszczeniem chunków*/
    [Tooltip("Czas po jakim mają zostać usunięte wszystkie chunki (w sekundach)")] public float cull_timer;

    /*Słownik reprezentujący chunki wraz z ich współrzędnymi*/
    private Dictionary<Vector3Int, Chunk> chunks = new Dictionary<Vector3Int, Chunk>();

    private float time_since_last_cull = 0f;
    private Color biome; 
    private List<GameObject> currentBiomePrefabs;
    private int current_biome;

    private void Awake()
    {
        /*Ustawia pierwszy biome*/
        biome = new Color(0.0230f, 0.460f, 0.0958f);
        currentBiomePrefabs = biome_settings[0].Prefabs;
        current_biome = 0;
    }

    private void FixedUpdate()
    {
        CreateChunks();

        /*Aktualizuj licznik czasu od ostatniego czyszczenia chunków*/
        time_since_last_cull += Time.fixedDeltaTime;

        /*Jeśli upłynął wymagany czas, wykonaj czyszczenie chunków i zresetuj licznik czasu*/
        if (time_since_last_cull >= cull_timer)
        {
            CullChunks();
            current_biome = (current_biome + 1) % 3; //Oblicza który biome ma zostać utworzony przez
            ChangeBiome(current_biome);
            time_since_last_cull = 0f;
        }
    }

    private void CreateChunks()
    {
        /*Obliczanie współrzędnych chunka dla wektora INTÓW (Np przejście z chunka (0, 0, 0) -> (0, 0, 1))*/
        var player_cord = Vector3Int.RoundToInt(player.transform.position / chunk_prefab.size);

        /*Pętla tworząca chunki*/
        foreach (var pos in PositionsAround(player_cord))
        {
            CreateNewChunk(pos, chunk_prefab);
        }
    }

    private Chunk CreateNewChunk(Vector3Int position, Chunk prefab)
    {
        if (chunks.ContainsKey(position)) return default; //Jeżeli chunki istnieją to nie rób nic
        var instance = Instantiate(prefab.gameObject);
        instance.transform.position = (Vector3) position * chunk_prefab.size; //Oblicza pozycję chunka
        Chunk chunkInstance = instance.GetComponent<Chunk>();
        chunkInstance.Paint(biome); //Koloruje chunki
        GenerateRandomObjectsOnChunk(chunkInstance); //Generowanie obiektów na chunku
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
        return distanceX <= 2 && distanceZ <= 2;
    }

    /* Funkcja dodająca do współrzędnych pierwszych chunków pozycję gracza 
            w celu ustalenia współrzędnych dla nowych fragmentów           */
    private Vector3Int[] PositionsAround(Vector3Int position)
    {
        List<Vector3Int> positions = new List<Vector3Int>();

        for (int x = -5 / 2; x <= 5 / 2; x++)
        {
            for (int z = -5 / 2; z <= 5 / 2; z++)
            {
                positions.Add(position + new Vector3Int(x, 0, z));
            }
        }

        return positions.ToArray();
    }
    
    /*Funkcja zmieniająca wartość zmiennej ustalającej jaki biome ma być generowany*/
    private void ChangeBiome(int which_biome)
    {
        if (which_biome == 0)
        {
            biome = new Color(0.0230f, 0.460f, 0.0958f);
            currentBiomePrefabs = biome_settings[which_biome].Prefabs;
        } 
        else if (which_biome == 1) 
        {
            biome = new Color(0.911f, 0.916f, 0.920f);
            currentBiomePrefabs = biome_settings[which_biome].Prefabs;
        }
        else if (which_biome == 2) 
        {
            biome = new Color(0.750f, 0.716f, 0.525f);
            currentBiomePrefabs = biome_settings[which_biome].Prefabs;
        }
    }

    /*Funkcja generująca prefaby (obiekty) w losowych miejscach na chunku*/
    private void GenerateRandomObjectsOnChunk(Chunk chunk)
    {
        foreach (var prefab in currentBiomePrefabs)
        {
            Vector3 randomPosition = chunk.transform.position + new Vector3(Random.Range(-chunk_prefab.size / 2f, chunk_prefab.size / 2f),
                0f, Random.Range(-chunk_prefab.size / 2f, chunk_prefab.size / 2f));
            Instantiate(prefab, randomPosition, Quaternion.Euler(-90, 0, 0), chunk.transform);
        }
    }
}
