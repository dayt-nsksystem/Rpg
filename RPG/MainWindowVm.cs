using System;
using System.Windows;
using System.Windows.Media;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaterialDesignThemes.Wpf;
using NskWpfFramework1._0.ComponentModel;
using Reactive.Bindings;
using Prism.Commands;
using Prism.Mvvm;
using System.ComponentModel;
using System.Data;
using System.Threading;
using System.Windows.Data;

namespace RPG
{
    public class MainWindowVm : BindableBase
    {
        #region プロパティ

        /// <summary>
        /// 攻撃コマンドが選択されているか
        /// </summary>
        public bool IsCheckedAttackCommand
        {
            get => _isCheckedAttackCommand;
            set => SetProperty(ref _isCheckedAttackCommand, value, nameof(IsVisibilityTargetSelectBox));
            // 第3引数を指定すると、nameofで指定したプロパティも同時に通知してくれる
            // 通知したいプロパティが大量にあるときは RaisePropertyChanged(nameof(なんか通知したいやつ)); で通知することができる
        }
        private bool _isCheckedAttackCommand = true;

        /// <summary>
        /// ターゲット選択ボックスを表示するかどうか
        /// </summary>
        /// Xamlのオブジェクトの表示非表示を制御するVisibilityプロパティは列挙型であるため
        /// 文字列で"Visible"と書くよりSystem.Window.Visibility.value.ToString()のように
        /// コードで補った方がタイプミスを防ぐことができ、バグ防止に役立てることができる
        public string IsVisibilityTargetSelectBox => IsCheckedAttackCommand ? Visibility.Visible.ToString() : Visibility.Collapsed.ToString();

        /// <summary>
        /// 敵キャラのリスト
        /// </summary>
        public ObservableCollection<Enemy> Enemies { get; set; } = new ObservableCollection<Enemy>();

        /// <summary>
        /// 選択された敵キャラ
        /// </summary>
        /// 攻撃対象にされてしまった敵キャラ
        public Enemy SelectedEnemy { get; set; }

        #endregion

        /// <summary>
        /// <see cref="MainWindowVm"/>コンストラクタ
        /// </summary>
        /// ウィンドウが立上がった一番最初に実行されるメソッド
        /// 主にオブジェクトの生成やパラメータの初期化などを行う
        public MainWindowVm()
        {
            BindingOperations.EnableCollectionSynchronization(Enemies, new object());

            Enemies.Add(new Enemy());
            Enemies.Add(new Enemy());

            Task.Run(() =>
            {
                while (true)
                {
                    Thread.Sleep(1000);
                    Enemies.Add(new Enemy());
                }
            });
        }


        #region AttackCommand　攻撃コマンド

        public DelegateCommand AttackCommand =>
                    _AttackCommand ?? (_AttackCommand = new DelegateCommand(ExecuteAttack));
        private DelegateCommand _AttackCommand;

        void ExecuteAttack()
        {
            SelectedEnemy.Hp -= 10;
            if (SelectedEnemy.Hp <= 0) Enemies.Remove(SelectedEnemy);
        }

        #endregion
    }

    /// <summary>
    /// 主人公クラス
    /// </summary>
    public class Player : BindableBase
    {
        #region パラメーター

        /// <summary>
        /// ヒットポイント
        /// </summary>
        public int Hp { get; set; }

        /// <summary>
        /// 経験値
        /// </summary>
        public int Exp { get; set; }

        #endregion

    }

    /// <summary>
    /// 敵クラス
    /// </summary>
    public class Enemy : BindableBase
    {
        #region 敵キャラの各パラメーター

        public int Hp
        {
            get => _hp;
            set => SetProperty(ref _hp, value);  // SetProperty()を用いることで自動的に通知してくれるようになる
        }
        private int _hp;

        public int Atc { get; set; }

        public int Def { get; set; }

        /// <summary>
        /// 敵キャラの色
        /// </summary>
        public string EnemyColor { get; set; }

        /// <summary>
        /// 敵キャラの形
        /// </summary>
        public PackIconKind EnemyShape { get; set; }

        #endregion

        /// <summary>
        /// 色の選択肢
        /// </summary>
        private readonly Color[] _colorOption = new Color[] { Colors.Red, Colors.Green, Colors.Blue, Colors.Yellow, Colors.Aqua };

        /// <summary>
        /// 形の選択肢
        /// </summary>
        private readonly PackIconKind[] _shapeOption = new PackIconKind[] { PackIconKind.Bacteria, PackIconKind.Blinky, PackIconKind.Bug };

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// コンストラクタとは…
        /// new演算子によってインスタンスが生成された時に自動的に実行されるメソッド。
        public Enemy()
        {
            Random r = new Random(DateTime.Now.Millisecond + DateTime.Now.Second);
            Thread.Sleep(10);

            // 色と形をランダムに決定する
            EnemyColor = _colorOption[r.Next(0, _colorOption.Length - 1)].ToString();
            EnemyShape = _shapeOption[r.Next(0, _shapeOption.Length - 1)];

            Hp = r.Next(80, 120);
        }
    }
}
