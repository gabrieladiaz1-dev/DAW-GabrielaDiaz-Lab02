namespace GestionCalidad.ViewModels
{
    public class DashboardViewModel
    {
        public int TotalInspecciones { get; set; }
        public int Aprobadas { get; set; }
        public int Rechazadas { get; set; }
        public int Pendientes { get; set; }
        public double PorcentajeAprobacion { get; set; }
        public double PorcentajeRechazo { get; set; }
        public double PorcentajePendiente { get; set; }
    }
}
