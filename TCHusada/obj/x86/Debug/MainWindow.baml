   M S B A M L   `   `   `  ����  TCHusada    TCHusada.MainWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5        $��utama��5   	     � Title$  
MainWindow��5   	   $	��620��6   $	��341��6)   
������  � WindowStyle$
 None=�5   "    J� OpacityMask
 ����� � AllowsTransparency .  � WindowStartupLocation$ CenterScreen=�5   #    � Icon$  /TCHusada;component/DB.ico��6H    � Triggers 5	      :� 5
       :� RoutedEvent$ Window.Loaded��6   .[�5      �� .��5      �� -   5         g�Name$ FormFade��5      .��5      W� -   5        $ FormFadeAnimation��5   "   	 �� 
TargetName$	 utama��5   -   
 �� TargetProperty$
 (Window.Opacity)�5   -    W� From$	 0.0X�5   -   	 W� To$	 1.0X�68    g� Duration$ 0:0:1D�6A    g� AutoReverse .   g� RepeatBehavior$ 1x��5   A   5      5      5   	   :� 5      $ Window.Unloaded��6   .[�5      �� .��5      �� -   5        $ FormFadeOut��5      .��5      W� -   5        $ FormFadeOutAnimation��5   "   $	 utama��5   -   $
 (Window.Opacity)�5   -   $	 1.0X�5   -   $	 0.0X�68   $ 0:0:1D�6A    .  $ 1x��5   A   5      5       5!   	   .��5$      � .+�5%      �� -   5        $��imgBackground��5%      $
��Fill=�6#   $��iponkecil.png��62   $��0,0,0,0q�6I   �� 5&      -   5        $��lblUserName��5&   .   $��34��6   $��
0,103,76,0q�6   $	��Top=�6A   $
��Bold"�6Y   
�������$��18(�6~   $��Sistem Informasi��6�   $��Right=�6�   $	��151��6�   �� 5'      -   5        $��label1��5'   �   $��Rumah Sakit TC Husada��6   $��18(�6.   $
��Bold"�6<   
���   �$��34��6a   $��43,153,38,0q�6m   $	��Top=�6�   $��Center=�6�    � Cursor$
 Help��6�    � ToolTip$ Tentang aplikasi ini��6�   �� 5(      -	   5        $��label2��5(   p   $
��Exit��6   $��13(�6   $
��Bold"�6+   
�������$��29��6P   $��
43,108,0,0q�6\   $	��Top=�6~   $
 Hand��6�   $
��Left=�6�   $��53��6�   $ Keluar dari aplikasi��6�   �� 5)      -
   5        $��txtUserName��5)   L   $��37��6   $��Right=�6   $��
0,262,45,0q�68   $	��Top=�6_   $	��252��6w   $��18(�6�   $��Username��6�   $ Masukkan username anda��6  �� 5*      -   5        $��image1��5*   J   $��60��6   $
��Left=�6   $��144,537,0,0q�65   $
��Fill=�6X   $	��Top=�6g   $��60��6   $"��/TCHusada;component/tomb.png��6�   $
 Hand��6�   $ Keluar dari aplikasi��6�   �� 5+      -   5        $��label3��5+   \   $��Username��6   $��30��6!   $
��Left=�6-   $��
36,226,0,0q�6H   $	��Top=�6j   $��18(�6�   �� 5,      -   5        $��label4��5,   \   $��Password��6   $��30��6!   $
��Left=�6-   $��
36,317,0,0q�6H   $	��Top=�6j   $��18(�6�   G� 5-      -   5        $��passwordBox1��5-   P   $��37��6   $��Right=�6    $��
0,353,45,0q�6<   $	��Top=�6d   $	��252��6|   $��18(�6�    G� Password$ Password��6�   $ Masukkan password anda��6  �� 5.      -   5        $��label5��5.   �   $��Login��6   $��18(�6   $
��Bold"�6,   
�������$��45��6Q   $��Center=�6]   $��43,470,38,0q�6�   $	��Top=�6�   $
 Hand��6�   $ Masuk��6�   �� 5/      -   5        $��labeljam��5/   Y   $��00:00��6   $��28��6   $
��Left=�6*   $��
150,80,0,0q�6E   $	��Top=�6i   $
��Bold"�6�   50      51      