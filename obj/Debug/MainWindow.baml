   M S B A M L   `   `   `  ����O  Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem  5   	   N JSharpDX, Version=2.6.3.0, Culture=neutral, PublicKeyToken=b4dcf0f35e5521f12&clr-namespace:SharpDX;assembly=SharpDXSharpDX 5   	   M ISystem, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089A-clr-namespace:System.IO.Ports;assembly=SystemSystem.IO.Ports 5	   	   @ <AIBot, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:AIBotAIBot 5
   	      AIBot.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   6sys&clr-namespace:System;assembly=mscorlib     5   	   6sdx&clr-namespace:SharpDX;assembly=SharpDX     5   	   d `HelixToolkit.Wpf.SharpDX, Version=2015.1.686.0, Culture=neutral, PublicKeyToken=52aa3500039caf0d-hx$http://helix-toolkit.org/wpf/SharpDX  5   	   9ports-clr-namespace:System.IO.Ports;assembly=System  5	   	   localclr-namespace:AIBot  5
   	     � Title$'  !Hexapod Controller V0.1 By Comdet��5   	   $��503.219��63   $��811.91��6D    � 
ResizeMode$ NoResize=�6S    � DataContext 5        AIBot.MainViewModel  5   
    � 	Resources 5      !  AIBot.BoolToStringConverter   BooleanToStringConverter�  �AIBot.Display.BoolToValueConverter`1[[System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]   
FalseValue   	TrueValue7  1HelixToolkit.Wpf.SharpDX.RenderTechniqueConverter  RenderTechniqueConverter S� 
ObjectType   System.IO.Ports.SerialPort S� 
MethodName  	portNames��%�   &	        &	 7     &	 J       5   
   $ Connect��6G   $ 
Disconnect��6\     5   
   6&   S� 5   
   "  6   $ GetPortNames��6D   6^   5      .��5      �  � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      .+�5   
   o� $��0��6   $	��Top=�6   ��5      �� 5      	 �� Opacity$		 0.5Q�6"   
 �� Color$
 
WhiteSmoke��60   .@�5       m� $
��View��6   .@�5!      m� $��Zoom extents��6    m� Command$% hx:ViewportCommands.ZoomExtents��61   5"      5#      *  $HelixToolkit.Wpf.SharpDX.Viewport3DX  5%   
   -   5          Name$ view1��5'      $��1��5&         	TextBrush
 �   �   ShowCoordinateSystem .  &   CoordinateSystemLabelForeground
 �����   UseDefaultGestures .     Camera 5(      �� *Camera+   Title 5)      �� *Title+   SubTitle 5*      �� *
SubTitle+   RenderTechnique 5/      �� *RenderTechnique+ J� InputBindings 50      �� 51      
 �� Key$ B��6    �� Command$" hx:ViewportCommands.BackView��6%   �� 52      $ F��6   $# hx:ViewportCommands.FrontView��6%   �� 53      $ U��6   $! hx:ViewportCommands.TopView��6%   �� 54      $ D��6   $$ hx:ViewportCommands.BottomView��6%   �� 55      $ L��6   $" hx:ViewportCommands.LeftView��6%   �� 56      $ R��6   $# hx:ViewportCommands.RightView��6%   �� 57       �� Gesture$ 	Control+E��6   $% hx:ViewportCommands.ZoomExtents��61   d� 58       d� Gesture$ 
RightClicka�6   $  hx:ViewportCommands.Rotate��64   d� 59      $ MiddleClicka�6   $ hx:ViewportCommands.Zoom��65   d� 5:      $ Shift+RightClicka�6   $ hx:ViewportCommands.Pan��6:   .@�5<      -  'HelixToolkit.Wpf.SharpDX.AmbientLight3D  &   HelixToolkit.Wpf.SharpDX.Light3D   Color 6    �� *AmbientLightColor+1	  +HelixToolkit.Wpf.SharpDX.DirectionalLight3D	  5=       6$   �� *DirectionalLightColor+   	Direction 6L   �� *DirectionalLightDirection+2
  ,HelixToolkit.Wpf.SharpDX.LineGeometryModel3D
  5@      -   5         
 Name$ bodyline��5@   %     SharpDX.Color
   Red 
  Color# Z 6�    
  	Thickness$ 2Q�6�   .  (HelixToolkit.Wpf.SharpDX.GeometryModel3D    Geometry  68   �� *HexModel.ModelSkeleton+&   HelixToolkit.Wpf.SharpDX.Model3D!   	Transform! 6d   �� *HexModel.SkeletonTransform+6�   
  5A      -   5        $
 leg1��5A   %   "   Blue# Z" 6�   $ 2Q�6�     64   �� *HexModel.Leg1.Leg+! 6[   �� *HexModel.Leg1.Transform+6�   
  5B      -   5        $
 leg2��5B   %   # Z" 6�   $ 2Q�6�     64   �� *HexModel.Leg2.Leg+! 6[   �� *HexModel.Leg2.Transform+6�   
  5C      -   5        $
 leg3��5C   %   # Z" 6�   $ 2Q�6�     64   �� *HexModel.Leg3.Leg+! 6[   �� *HexModel.Leg3.Transform+6�   
  5D      -   5        $
 leg4��5D   %   # Z" 6�   $ 2Q�6�     64   �� *HexModel.Leg4.Leg+! 6[   �� *HexModel.Leg4.Transform+6�   
  5E      -   5        $
 leg5��5E   %   # Z" 6�   $ 2Q�6�     64   �� *HexModel.Leg5.Leg+! 6[   �� *HexModel.Leg5.Transform+6�   
  5F      -   5        $
 leg6��5F   %   # Z" 6�   $ 2Q�6�     64   �� *HexModel.Leg6.Leg+! 6[   �� *HexModel.Leg6.Transform+6�   
  5G      -	   5        $
 grid��5G   %   #   Gray# Z# 6x   $	 0.5Q�6�     65   �� *Grid+! 6P   �� *GridTransform+6�   4  .HelixToolkit.Wpf.SharpDX.UIRotateManipulator3D  5I      -
   5        $  Name$$ rotateY��5I   *   %   Axis9  3HelixToolkit.Wpf.SharpDX.Utilities.Vector3Converter$% 0,1,0 6;   -  'HelixToolkit.Wpf.SharpDX.PhongMaterials&   Green6  0HelixToolkit.Wpf.SharpDX.MaterialGeometryModel3D'   Material#' Z& 5J   !   5M      �� 5N   
   -   5        ( ��Name$( groupBox��5N      $��GroupBox��6%   $��	0,94,10,0q�67   $��1��6J   ���	#FF979797$	��150��6n   $	��150��6z   $��Right=�6�   $	��Top=�6�   .��5O      � $��0q�6   .+�5P      �� -   5        ) ��Name$) 	btForward��5P      $
��Forw��6,   $��Center=�6;   $��0,5,0,0q�6X   $	��Top=�6i   $��30��6�   $��30��6�   6�   �� 5Q      -   5        $) btBack��5Q      $
��Back��6)   $��Center=�68   $��0,0,0,5q�6U   $��Bottom=�6f   $��30��6�   $��30��6�   6�   �� 5R      -   5        $) btLeft��5R      $
��LEFT��6)   $
��Left=�68   $��5,0,0,0q�6S   $��Center=�6d   $��30��6   $��30��6�   6�   �� 5S      -   5        $) btRight��5S      $
��RIGT��6*   $��Right=�69   $��0,0,5,0q�6U   $��Center=�6f   $��30��6�   $��30��6�   6�   �� 5T      -   5        $) btBodyUp��5T      $	��BUP��6+   $
��Left=�69   $��5,5,0,0q�6T   $	��Top=�6e   $��30��6}   $��30��6�   6�   �� 5U      -   5        $) 
btBodyDown��5U      $��BDOWN��6-   $
��Left=�6=   $��5,0,0,5q�6X   $��Bottom=�6i   $��30��6�   $��30��6�   6�   �� 5V      -   5        $) 	btEndWalk��5V      $	��END��6,   $��Center=�6:   $��0q�6W   $��Center=�6b   $��30��6}   $��30��6�   6�   �� 5W      -   5        $) button7��5W      $��BL��6*   $��Right=�67   $��0,5,5,0q�6S   $	��Top=�6d   $��30��6|   $��30��6�   �� 5X      -   5        $) button8��5X      $��BR��6*   $��Right=�67   $��0,0,5,5q�6S   $��Bottom=�6d   $��30��6   $��30��6�   6�   5Y      5Z      �� 5[   
   $��1��6   .+�5\      9� -   5        * 9�Name$* shadingPanel��5\   %   $
��Port��6   $��Right=�6;   $	��100��6W   $	��Top=�6c   
�������+ 9� 
IsExpanded+ . $��10q�6�   ��5]      �� 5^      $
	 0.25Q�6&   $
 
WhiteSmoke��65   .��5`      �� $��5q�6   .+�5a      �� -   5        , ��Name$, comboPortName��5a      $	��Top=�66   $��25��6N   ��6Z   �� - �� Source#- [ �� 5b      -   5        $) 	btConnect��5b      $��25��60   $��0,5,0,0q�6<   $��//��6M   6Z   5c      5d      5e      �� 5g   
   $��1��6   $��Bottom=�6!   ��5h      �� 5i      $		 0.5Q�6"   $
 
WhiteSmoke��60   .@�5k      �� .��5l      �� $	��Top=�6   $
��Left=�64   ��6O   �� *FpsCounter.Value+. �� ElementName$. view1��/ �� StringFormat$/ D3D11 - {0:0.00} FPS��5m      �� 5n      �� 5o      .��5p      �� ��6   �� *Camera.Position+$/ Position: {0:0.0}��5q      �� 5r      �� 5s      .��5t      �� ��6   �� *Camera.LookDirection+$/ LookDirection: {0:0.0}��5u      �� 5v      �� 5w      .��5x      �� ��6   �� *Camera.UpDirection+$/ UpDirection: {0:0.0}��5y      �� 5z      �� 5{      .��5|      �� ��6   �� *Items.Count+$. view1��$/ Children: {0}��5}      5~      5�      5�      