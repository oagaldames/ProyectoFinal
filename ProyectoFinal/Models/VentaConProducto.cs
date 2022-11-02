namespace ProyectoFinal.Models
{
    public class VentaConProducto
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public int Cantidad { get; set; }
        public string Comentarios { get; set; }
        public int IdUsuario { get; set; }

        public VentaConProducto()
        {
            Id = 0;
            Descripcion = string.Empty;
            Comentarios = string.Empty;
            Cantidad = 0;
            IdUsuario = 0;
        }
        public VentaConProducto(int id,string descripcion,int cantidad, string comentarios, int idUsuario)
        {
            Id = id;
            Descripcion = descripcion;
            Cantidad=cantidad;
            Comentarios = comentarios;
            IdUsuario = idUsuario;
        }
    }
}
