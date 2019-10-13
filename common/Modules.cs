using Raster;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Utilities;

namespace Modules
{
  /// <summary>
  /// Notification handler for async operations.
  /// </summary>
  /// <param name="module"></param>
  public delegate void NotifyHandler (IRasterModule module);

  public interface IRasterModule
  {
    /// <summary>
    /// Author's full name.
    /// </summary>
    string Author { get; }

    /// <summary>
    /// Name of the module (short enough to fit inside a list-boxes, etc.).
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Tooltip for Param (text parameters).
    /// </summary>
    string Tooltip { get; }

    /// <summary>
    /// Current 'Param' string is stored in the module.
    /// Set reasonable initial value.
    /// </summary>
    string Param { get; set; }

    /// <summary>
    /// Optional mouse handler: button down
    /// </summary>
    MouseEventHandler MouseDown { get; }

    /// <summary>
    /// Optional mouse handler: button up
    /// </summary>
    MouseEventHandler MouseUp { get; }

    /// <summary>
    /// Optional mouse handler: pointer move
    /// </summary>
    MouseEventHandler MouseMove { get; }

    /// <summary>
    /// Optional mouse handler: wheel turn
    /// </summary>
    MouseEventHandler MouseWheel { get; }

    /// <summary>
    /// Optional keyboard handler
    /// </summary>
    KeyEventHandler KeyDown { get; }

    // Mandatory plain module-instance constructor should look like
    // class Module : IRasterModule
    // {
    //   public Module ()
    //   {
    //     ...
    //   }
    //   ...
    // }

    /// <summary>
    /// Usually read-only, optionally writable (client is defining number of inputs).
    /// </summary>
    int InputSlots { get; set; }

    /// <summary>
    /// Assigns an input raster image to the given slot.
    /// Doesn't start computation (see #Update for this).
    /// </summary>
    /// <param name="inputImage">Input raster image (can be null).</param>
    /// <param name="slot">Slot number from 0 to InputSlots-1.</param>
    void SetInput (
      Bitmap inputImage,
      int slot = 0);

    /// <summary>
    /// Assigns an input HDR image to the given slot.
    /// Doesn't start computation (see #Update for this).
    /// </summary>
    /// <param name="inputImage">Input HDR image (can be null).</param>
    /// <param name="slot">Slot number from 0 to InputSlots-1.</param>
    void SetInput (
      FloatImage inputImage,
      int slot = 0);

    /// <summary>
    /// Recompute the output image[s] according to input image[s].
    /// Blocking (synchronous) function.
    /// #GetOutput() functions can be called after that.
    /// </summary>
    void Update ();

    /// <summary>
    /// Asynchronous recomputing of the output image[s].
    /// </summary>
    /// <param name="notify">Optional notification handler.</param>
    void UpdateAsync (
      NotifyHandler notify = null
      );

    /// <summary>
    /// Optional action performed at the given pixel.
    /// Blocking (synchronous) function.
    /// Logically equivalent to Update() but with potential local effect.
    /// </summary>
    void PixelUpdate (
      int x,
      int y);

    /// <summary>
    /// Async action performed at the given pixel.
    /// Logically equivalent to UpdateAsync() but with potential local effect.
    /// </summary>
    /// <param name="notify">Optional notification handler.</param>
    void PixelUpdateAsync (
      int x,
      int y,
      NotifyHandler notify = null);

    /// <summary>
    /// Usually read-only, sometimes writable (client is defining number of outputs).
    /// </summary>
    int OutputSlots { get; set; }

    /// <summary>
    /// Returns an output raster image.
    /// Can return null.
    /// </summary>
    /// <param name="slot">Slot number from 0 to OutputSlots-1.</param>
    Bitmap GetOutput (
      int slot = 0);

    /// <summary>
    /// Returns an output HDR image.
    /// Can return null.
    /// </summary>
    /// <param name="slot">Slot number from 0 to OutputSlots-1.</param>
    FloatImage GetOutputHDR (
      int slot = 0);

    /// <summary>
    /// Returns true if there is an active GUI window associted with this module.
    /// Open/close GUI window using the setter.
    /// </summary>
    bool GuiWindow { get; set; }
  }

  public abstract class DefaultRasterModule : IRasterModule
  {
    /// <summary>
    /// Author's full name.
    /// Mandatory override.
    /// </summary>
    public abstract string Author { get; }

    /// <summary>
    /// Name of the module (short enough to fit inside a list-boxes, etc.).
    /// Mandatory override.
    /// </summary>
    public abstract string Name { get; }

    /// <summary>
    /// Tooltip for Param (text parameters).
    /// </summary>
    public virtual string Tooltip => "-- no params --";

    /// <summary>
    /// Param string.
    /// Default behavior: no parameter.
    /// </summary>
    public virtual string Param { get; set; } = "";

    /// <summary>
    /// Optional mouse handler: button down
    /// </summary>
    public virtual MouseEventHandler MouseDown => null;

    /// <summary>
    /// Optional mouse handler: button up
    /// </summary>
    public virtual MouseEventHandler MouseUp => null;

    /// <summary>
    /// Optional mouse handler: pointer move
    /// </summary>
    public virtual MouseEventHandler MouseMove => null;

    /// <summary>
    /// Optional mouse handler: wheel turn
    /// </summary>
    public virtual MouseEventHandler MouseWheel => null;

    /// <summary>
    /// Optional keyboard handler
    /// </summary>
    public virtual KeyEventHandler KeyDown => null;

    /// <summary>
    /// Usually read-only, optionally writable (client is defining number of inputs).
    /// </summary>
    public virtual int InputSlots
    {
      get => InputSlots;
      set => InputSlots = 1;
    }

    /// <summary>
    /// Assigns an input raster image to the given slot.
    /// Doesn't start computation (see #Update for this).
    /// </summary>
    /// <param name="inputImage">Input raster image (can be null).</param>
    /// <param name="slot">Slot number from 0 to InputSlots-1.</param>
    public virtual void SetInput (
      Bitmap inputImage,
      int slot = 0)
    {}

    /// <summary>
    /// Assigns an input HDR image to the given slot.
    /// Doesn't start computation (see #Update for this).
    /// </summary>
    /// <param name="inputImage">Input HDR image (can be null).</param>
    /// <param name="slot">Slot number from 0 to InputSlots-1.</param>
    public virtual void SetInput (
      FloatImage inputImage,
      int slot = 0)
    {}

    /// <summary>
    /// Recompute the output image[s] according to input image[s].
    /// Blocking (synchronous) function.
    /// #GetOutput() functions can be called after that.
    /// </summary>
    public virtual void Update ()
    {}

    /// <summary>
    /// Asynchronous recomputing of the output image[s].
    /// </summary>
    /// <param name="notify">Optional notification handler.</param>
    public virtual void UpdateAsync (
      NotifyHandler notify = null
      )
    {
      // !!! TODO: default async implementation !!!
    }

    /// <summary>
    /// Optional action performed at the given pixel.
    /// Blocking (synchronous) function.
    /// Logically equivalent to Update() but with potential local effect.
    /// </summary>
    public virtual void PixelUpdate (
      int x,
      int y)
    {}

    /// <summary>
    /// Async action performed at the given pixel.
    /// Logically equivalent to UpdateAsync() but with potential local effect.
    /// </summary>
    /// <param name="notify">Optional notification handler.</param>
    public virtual void PixelUpdateAsync (
      int x,
      int y,
      NotifyHandler notify = null)
    {
      // !!! TODO: default async implementation !!!
    }

    /// <summary>
    /// Usually read-only, sometimes writable (client is defining number of outputs).
    /// </summary>
    public virtual int OutputSlots
    {
      get => OutputSlots;
      set => OutputSlots = 1;
    }

    /// <summary>
    /// Returns an output raster image.
    /// Can return null.
    /// </summary>
    /// <param name="slot">Slot number from 0 to OutputSlots-1.</param>
    public virtual Bitmap GetOutput (
      int slot = 0) => null;

    /// <summary>
    /// Returns an output HDR image.
    /// Can return null.
    /// </summary>
    /// <param name="slot">Slot number from 0 to OutputSlots-1.</param>
    public virtual FloatImage GetOutputHDR (
      int slot = 0) => null;

    /// <summary>
    /// Returns true if there is an active GUI window associted with this module.
    /// Open/close GUI window using the setter.
    /// </summary>
    public virtual bool GuiWindow
    {
      get => GuiWindow;
      set => GuiWindow = false;
    }
  }

  public class ModuleRegistry
  {
    /// <summary>
    /// Module.Name => prototype class.
    /// </summary>
    internal static Dictionary<string, Type> reg = new Dictionary<string, Type>();

    /// <summary>
    /// Register a new module.
    /// </summary>
    public static void RegisterModules ()
    {
      IEnumerable<Type> compatible = TypeLoader.GetTypesWithInterface<DefaultRasterModule>();
      foreach (Type t in compatible)
      {
        var constructor = t.GetConstructor(null);
        if (constructor != null)
        {
          IRasterModule rm = constructor.Invoke(null) as IRasterModule;
          if (rm != null)
            reg[rm.Name] = t;
        }
      }
    }

    public static IRasterModule CreateModule (string name)
    {
      if (reg.ContainsKey(name))
      {
        var constructor = reg[name].GetConstructor(null);
        if (constructor != null)
        {
          IRasterModule rm = constructor.Invoke(null) as IRasterModule;
          if (rm != null)
            return rm;
        }
      }

      return null;
    }

    public static ICollection<string> RegisteredModuleNames ()
    {
      return reg.Keys;
    }
  }
}