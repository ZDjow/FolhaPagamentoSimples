namespace FolhaPagamentoSimples.Presentation
{
    static class GUI
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FmGUI());
        }
    }
}
