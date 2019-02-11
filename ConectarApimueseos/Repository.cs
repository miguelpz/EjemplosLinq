using ConectarApimueseos.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ConectarApimueseos
{
    public class Repository
    {
        JArray posts;
        JArray comments;
        JArray albums;
        JArray photos;
        JArray todos;
        JArray users;

        public Repository()
        {

            posts = JArray.Parse(connectTo("posts"));
            comments = JArray.Parse(connectTo("comments"));
            albums = JArray.Parse(connectTo("albums"));
            photos = JArray.Parse(connectTo("photos"));
            todos = JArray.Parse(connectTo("todos"));
            users = JArray.Parse(connectTo("users"));

            mostrarTareasTerminadas();
        }

        //static async void Lee()
        //{
        //    JArray posts;
        //    HttpClient Cliente = new HttpClient();
        //    String url = @"http://jsonplaceholder.typicode.com/posts";
        //    var uri = new Uri(url);

        //    var response = await Cliente.GetAsync(uri);
        //    if (response.IsSuccessStatusCode)
        //    {
        //        var content = await response.Content.ReadAsStringAsync();
        //        posts = JArray.Parse(content);

        //    }
        //}


        // ver cuales son los comentarios que hay para un post determinado.
        public void mostrarComentarios (int postId)
        {
            var commentarios = from c in comments
                               where (int)c["postId"] == postId                            
                               select c;
        }

        // ver cuales son los albums que hay y la cantidad de fotos que contiene cada uno de ellos
        public void mostrarAlbunesConFotos()
        {
            var resultado = from a in albums
                            join p in photos on (int)a["id"] equals (int)p["albumId"]
                            group a by a["title"] into grupo
                            select new { Titulo = grupo.Key, TotalFotos = grupo.Count() };                                               
        }

        //ver cuales son los nombres de los usuarios ordenados descendentemente.
        public void mostrarUsuariosDecendente()
        {
            var resultado = from u  in users
                            orderby u["username"] descending
                            select u["username"];

        }

        //Tomar los cinco albums donde más fotos existan.
        public void mostrarCincoAlbumsMasFotos()
        {
            var resultado = (from a in albums
                            join p in photos on (int)a["id"] equals (int)p["albumId"]                          
                            group a by a["title"] into grupo
                            orderby  grupo.Key.Count() descending
                            select new { Titulo = grupo.Key, TotalFotos = grupo.Count() }).Take(5);
        }

        //Ver que cantidad de comentarios totales ha recibido un usuario.
        public void mostrarCantidadComentariosPorUsuario()
        {
            var resultado = from c in comments
                            join u in users on (int)c["postId"] equals (int)u["id"]
                            group c by u["name"] into grupo
                            select new { Usuario = grupo.Key, TotalComentarios = grupo.Count() };

        }

        //Mostrar el usuario que más comentarios ha recibido.
        public void mostrarUsuarioMasComentarios()
        {
            var resultado = (from c in comments
                            join u in users on (int)c["postId"] equals (int)u["id"]
                            group c by u["name"] into grupo
                            orderby grupo.Count() descending
                            select new { Usuario = grupo.Key, TotalComentarios = grupo.Count() }).Take(1);
        }

        //Mostrar para cada usuario cuantas tareas tiene terminada y cuales no.
        public void mostrarTareasTerminadas()
        {
            var resultado = from u in users
                            join t in todos on (int)u["id"] equals (int)t["userId"]
                            group t by new { nombre = u["name"] } into grupo
                            select new
                            {
                                Usuario = grupo.Key.nombre,
                                TotalAcabadas = grupo.Where(g => (bool)g["completed"] == false).Count(),
                                TotalSinTerminar = grupo.Where(g => (bool)g["completed"] == true).Count()
                            };





                         
                            
                            
                            



        }










        private string connectTo(string terminacion)
        {
            string json;

            String url = @"http://jsonplaceholder.typicode.com/" + terminacion;
            var uri = new Uri(url);

            HttpWebRequest RateRequest = (HttpWebRequest)WebRequest.Create(@uri);

            using (HttpWebResponse response = (HttpWebResponse)RateRequest.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                json = reader.ReadToEnd();
            }

            return json;
        }

        





        // var museos = objeto.SelectTokens("$.[?(@.visita=='S')]");


        //foreach (JObject item in objeto)
        //{
        //    var museo = 

        //    ListaMuseos.Add(museo);
        //}





        //      //  ListaMuseos = JsonConvert.DeserializeObject<List<Museo>>(content);
        //    }

        //    //foreach(var mus in ListaMuseos)
        //    //{
        //    //    Console.WriteLine(mus.Descripcion);
        //    //}

        //    Console.ReadLine();

        //}

        //        public async Task<string> getData (string terminacion)
        //{

        //    String url = @"jsonplaceholder.typicode.com/" + terminacion ;
        //    var uri = new Uri(url);

        //    using (HttpClient connection = new HttpClient())
        //    {
        //       var response =  await connection.GetAsync(uri);

        //        if (response.IsSuccessStatusCode)
        //        {
        //           return await response.Content.ReadAsStringAsync();


        //        }
        //        return null;
        //    }      
        //}







    }
}
