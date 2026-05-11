namespace WebApp.Models
{
    public class CompraInternacional
    {
        public int Id { get; set; }
        public DateOnly? FechaCompra { get; set; }
        public string Contract { get; set; }
        public string Container { get; set; }
        public string Sello { get; set; }
        public string Proveedor { get; set; }
        public string Origen { get; set; }
        public string Puerto { get; set; }
        public string Naviera { get; set; }
        public DateTime? ETD { get; set; }
        public DateTime? ETA { get; set; }
        public string Cruce { get; set; }
        public string Incoterm { get; set; }

        // Additional fields
        public string Invoice { get; set; }
        public string Pedimento { get; set; }
        public string ConexionesDemoras { get; set; }
        public DateTime? FechaEstimDestino { get; set; }
        public DateTime? UltimoDiaLibre { get; set; }
        public DateTime? EntregaVacio { get; set; }
        public int? DiasDemoras { get; set; }

        // Demoras section
        public string FacturaDemoras { get; set; }
        public decimal? MontoUSDDemoras { get; set; }
        public decimal? TCDemoras { get; set; }
        public decimal? MontoDemorasMXN { get; set; }

        // Documentos y fechas section
        public string FacturaComercializadora { get; set; }
        public DateTime? FechaDespacho { get; set; }
        public DateTime? FechaPagoPedimento { get; set; }
        public int? Planta { get; set; }

        // Producción y lote section
        public string Lote { get; set; }
        public DateTime? FechaProduccion { get; set; }
        public DateTime? FechaCaducidad { get; set; }
        public string Empresa { get; set; }
        public string Codigo { get; set; }
        public string DescripcionProducto { get; set; }
        public string Producto { get; set; }
        public string Marca { get; set; }
        public string Talla { get; set; }
        public decimal? Kgs { get; set; }
        public int? Cajas { get; set; }

        public class CatalogoItem
        {
            public string Codigo { get; set; } = string.Empty;
            public string Producto { get; set; } = string.Empty;
            public string? Talla { get; set; }
        }

    }
}
