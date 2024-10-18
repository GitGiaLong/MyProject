using System.Windows.Media.Effects;

namespace Core.Libraries.WPF.Interactivities
{
    public abstract class TransitionEffect : ShaderEffect
    {
        #region Dependency Properties

        public Brush Input
        {
            get { return (Brush)GetValue(InputProperty); }
            set { SetValue(InputProperty, value); }
        }
        public static readonly DependencyProperty InputProperty = ShaderEffect.RegisterPixelShaderSamplerProperty(nameof(Input), typeof(TransitionEffect), 
            0, SamplingMode.NearestNeighbor);

        public Brush OldImage
        {
            get { return (Brush)GetValue(OldImageProperty); }
            set { SetValue(OldImageProperty, value); }
        }
        public static readonly DependencyProperty OldImageProperty = ShaderEffect.RegisterPixelShaderSamplerProperty(nameof(OldImage), typeof(TransitionEffect), 
            1, SamplingMode.NearestNeighbor);

        public double Progress
        {
            get { return (double)GetValue(ProgressProperty); }
            set { SetValue(ProgressProperty, value); }
        }
        public static readonly DependencyProperty ProgressProperty = DependencyProperty.Register(nameof(Progress), typeof(double), 
            typeof(TransitionEffect), new PropertyMetadata(0.0, PixelShaderConstantCallback(0)));

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
    }
}
