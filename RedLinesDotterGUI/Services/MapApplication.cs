using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Map = MapInfoWrap;

namespace RedLinesDotterGUI.Services
{
    public sealed class MapApplication
    {
        private static readonly Lazy<MapApplication> _instance =
            new Lazy<MapApplication>(() => new MapApplication());

        MapApplication()
        {
            /// TODO: СДелать инициализацию мапинфо асинхронной!
            /// Порядок следующий: инициализация мэйн виндов |>
            /// Параллельно инициализация МапАппликэйшн |> 
            /// Когда апп инициализирован |> забить дата контекст!
            Initialize();
        }

        public bool IsConnected { get; private set; }
        public Map.MapInfoAppControls App { get; private set; }

        private void Initialize()
        {
            App = Map.MapInfoAppControls.Create();
            IsConnected = true;
        }

        private async Task InitializeAsync()
        {
            /// TODO: Довести до ума асинхронную инициализацию мапинфо
            try
            {
                var task = Task.Run(() =>
                {
                    App = Map.MapInfoAppControls.Create();
                    IsConnected = true;
                    Console.WriteLine("init before delay");
                    System.Threading.Thread.Sleep(10000);
                    Console.WriteLine("init after delay");
                }).ConfigureAwait(false);
                await task;
            }
            catch (Exception e)
            {
                IsConnected = false;
                throw new Exception("Ошибка инициализации", e);
            }
        }

        public static MapApplication Instance { get => _instance.Value; }
    }
}
