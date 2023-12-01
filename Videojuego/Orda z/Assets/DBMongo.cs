using System.Collections;
using System.Collections.Generic;
using MongoDB.Bson;
using UnityEngine;
using MongoDB.Driver;

public class DBMongo : MonoBehaviour
{
    private MongoClient client; //Conexion 
    private IMongoDatabase db;
    private IMongoCollection<ModelPlayer> collection;
    //private IMongoCollection<BsonDocument> collection;
    public static DBMongo DBMongoInstance;

    private void Awake()
    {
        if (DBMongoInstance == null)
        {
            DBMongoInstance = this;
            DontDestroyOnLoad(DBMongoInstance);
        }
        else
        {
            Destroy(this.gameObject);
        }

    }

    void Start()
    {
        client = new
MongoClient("mongodb+srv://ricardo:linux@cluster0.fkzcm0d.mongodb.net/?retryWrites=true&w=majority");
        db = client.GetDatabase("Unity");
        collection = db.GetCollection<ModelPlayer>("player");
        Debug.Log("Insertando Score");

    }

    public void Registrar(string Usuario, int Score)
    {
        var newPlayer = new ModelPlayer
        {
            Nickname = Usuario,
            Score = Score// Asegúrate de manejar las contraseñas de manera segura (hashing, salting, etc.)
                         // Otros campos del jugador
        };
        //var document = new BsonDocument { { "Name", Usuario }, { "score", Score } };
        collection.InsertOne(newPlayer);
    }

    public void ActualizarScore(string sNombre, int iPuntuacion)
    {
        if (client != null)
        {
            try
            {
                if (this.ObtenerIdPorName(sNombre) < iPuntuacion )
                {
                    // Define el filtro para identificar el documento que deseas actualizar
                    var filter = Builders<ModelPlayer>.Filter.Eq("Name", sNombre);
                    // Crea un documento de actualización con los cambios deseados
                    var update = Builders<ModelPlayer>.Update.Set("score", iPuntuacion);
                    // Ejecuta la operación de actualización
                    collection.UpdateOne(filter, update);
                }


            }
            catch (System.Exception e)
            {
                Debug.Log("ERROR: " + e.Message);
            }

        }

    }

    public int ObtenerIdPorName(string SNombre)
    {
        int score = -1;

        if (client != null)
        {
            try
            {
                var filter = Builders<ModelPlayer>.Filter.Eq("Name", SNombre);
                var result = collection.Find(filter).FirstOrDefault();

                if (result != null)
                {
                    score = result.Score;
                }
                else
                {
                    Debug.Log("No se encontró el jugador con el ID proporcionado.");
                }
            }
            catch (System.Exception e)
            {
                Debug.Log("ERROR: " + e.Message);
            }
            finally
            {
                
            }
        }
        return score;

    }
}
