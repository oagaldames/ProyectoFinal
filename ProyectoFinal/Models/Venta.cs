namespace ProyectoFinal.Models
{
    public class Venta
    {
        public int Id { get; set; }
        public string Comentarios { get; set; }
        public int IdUsuario { get; set; }

        public Venta()
        {
            Id = 0;
            Comentarios = string.Empty;
            IdUsuario = 0;
        }
        public Venta(int id, string comentarios, int idUsuario)
        {
            Id = id;
            Comentarios = comentarios;
            IdUsuario = idUsuario;
        }
    }
}
