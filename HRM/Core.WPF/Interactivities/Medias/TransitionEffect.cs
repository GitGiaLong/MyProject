using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace Core.WPF.Interactivities.Medias
{
    public abstract class TransitionEffect : ShaderEffect
    {
        #region Dependency Properties
        public static readonly DependencyProperty InputProperty = ShaderEffect.RegisterPixelShaderSamplerProperty("Input", typeof(TransitionEffect), 0, SamplingMode.NearestNeighbor);

        public static readonly DependencyProperty OldImageProperty = ShaderEffect.RegisterPixelShaderSamplerProperty("OldImage", typeof(TransitionEffect), 1, SamplingMode.NearestNeighbor);

        public static readonly DependencyProperty ProgressProperty = DependencyProperty.Register("Progress", typeof(double), typeof(TransitionEffect), new PropertyMetadata(0.0, PixelShaderConstantCallback(0)));

        #endregion

        #region Constructors

        public new TransitionEffect CloneCurrentValue()
        {
            return (TransitionEffect)base.CloneCurrentValue();
        }

        protected abstract TransitionEffect DeepCopy();

        protected TransitionEffect()
        {
            UpdateShaderValue(InputProperty);
            UpdateShaderValue(OldImageProperty);
            UpdateShaderValue(ProgressProperty);
        }

        #endregion

        public Brush Input
        {
            get { return (Brush)GetValue(InputProperty); }
            set { SetValue(InputProperty, value); }
        }

        public Brush OldImage
        {
            get { return (Brush)GetValue(OldImageProperty); }
            set { SetValue(OldImageProperty, value); }
        }

        public double Progress
        {
            get { return (double)GetValue(ProgressProperty); }
            set { SetValue(ProgressProperty, value); }
        }
    }
}
