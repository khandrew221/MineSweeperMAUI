using System.Xml;

namespace MineSweeperMAUI;

public partial class SettingsPage : ContentPage
{
    Label s_XSliderLabel;
    Label s_YSliderLabel;
    Label s_BombSliderLabel;

    Slider s_XSlider;
    Slider s_YSlider;
    Slider s_BombSlider;


    public SettingsPage()
	{
		InitializeComponent();

        //store relevant objects
        s_XSlider = (Slider)FindByName("XSlider");
        s_YSlider = (Slider)FindByName("YSlider");
        s_BombSlider = (Slider)FindByName("BombDensity");

        s_XSliderLabel = (Label)FindByName("XSliderValue");
        s_YSliderLabel = (Label)FindByName("YSliderValue");
        s_BombSliderLabel = (Label)FindByName("BombDensityValue");

        ReadSettings();
    }

    private void ReadSettings()
    {
        s_XSlider.Value = ((App)Application.Current).settings.Width;
        s_YSlider.Value = ((App)Application.Current).settings.Height;
        s_BombSlider.Value = (int)((App)Application.Current).settings.BombDensity * 100;

        UpdateLabels();
    }

    private void UpdateLabels()
    {
        s_XSliderLabel.Text = String.Format("{0}", (int)s_XSlider.Value);
        s_YSliderLabel.Text = String.Format("{0}", (int)s_YSlider.Value);
        s_BombSliderLabel.Text = String.Format("{0}%", (int)s_BombSlider.Value);

    }



    /// <summary>
    /// Controls slider code
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnSliderValueChanged(object sender, EventArgs e)
    {
        Slider s = (Slider)sender;
        String id = s.StyleId;

        switch (id)
        {
            case "BombDensity":
                s_BombSliderLabel.Text = String.Format("{0}%", (int)s_BombSlider.Value);
                ((App)Application.Current).settings.BombDensity = (float)(s_BombSlider.Value / 100.0);
                break;
            case "XSlider":
                s_XSliderLabel.Text = String.Format("{0}", (int)s_XSlider.Value);
                ((App)Application.Current).settings.Width = (int)(s_XSlider.Value);
                break;
            case "YSlider":
                s_YSliderLabel.Text = String.Format("{0}", (int)s_YSlider.Value);
                ((App)Application.Current).settings.Height = (int)(s_YSlider.Value);
                break;
        }

    }

}