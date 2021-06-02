using System;
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

namespace RPG
{
    public class MainWindowVm : NotifyValidatableObject //BindableBase
    {
        #region プロパティ

        /// <summary>
        /// 攻撃コマンドが選択されているか
        /// </summary>
        public bool IsCheckedAttackCommand
        {
            get => _isCheckedAttackCommand;
            set //=> SetProperty(ref _isCheckedAttackCommand, value, nameof(IsVisibilityTargetSelectBox));
            {
                _isCheckedAttackCommand = value;
                NotifyPropertyChanged(nameof(IsVisibilityTargetSelectBox));
            }
        }
        private bool _isCheckedAttackCommand = true;

        public string IsVisibilityTargetSelectBox => IsCheckedAttackCommand ? "Visible" : "Collapsed";

        /// <summary>
        /// 敵キャラのリスト
        /// </summary>
        public ObservableCollection<Enemy> Enemies
        {
            get => _enemies;
            private set => _enemies = value;
        }
        private ObservableCollection<Enemy> _enemies = new ObservableCollection<Enemy>();

        public Enemy SelectedEnemy { get; set; }

        #endregion

        public MainWindowVm()
        {
            Enemies.Add(new Enemy());
            Enemies.Add(new Enemy());
        }


        #region AttackCommand

        public DelegateCommand AttackCommand =>
                    _AttackCommand ?? (_AttackCommand = new DelegateCommand(ExecuteAttack));
        private DelegateCommand _AttackCommand;

        void ExecuteAttack()
        {
            SelectedEnemy.Hp -= 10;
            NotifyPropertyChanged(nameof(Enemies));
        }

        #endregion


    }

    /// <summary>
    /// 主人公クラス
    /// </summary>
    public class Player : NotifyValidatableObject
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
    public class Enemy : NotifyValidatableObject
    {
        #region 敵キャラの各パラメーター

        public int Hp { get; set; }

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
            // 色と形をランダムに決定する
            EnemyColor = _colorOption[new Random().Next(0, _colorOption.Length - 1)].ToString();
            EnemyShape = _shapeOption[new Random().Next(0, _shapeOption.Length - 1)];

            Hp = 100;
        }
    }
}
