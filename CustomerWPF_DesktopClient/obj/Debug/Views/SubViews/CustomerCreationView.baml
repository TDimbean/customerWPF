   M S B A M L   `   `   `  ����T  PCustomerWPF_DesktopClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullc6clr-namespace:CustomerWPF_DesktopClient.Views.SubViews(CustomerWPF_DesktopClient.Views.SubViews  5   	   C    =CustomerWPF_DesktopClient.Views.SubViews.CustomerCreationView   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   Hlocal6clr-namespace:CustomerWPF_DesktopClient.Views.SubViews     5   	   -   5          � Title$  Add a New Customer��5   	   $	��600��6$   $	��800��61   $��30(�6=    � 	Resources 5
       �� 
TargetType �� Property �� Value��%n  '��       '�� �     '��     �� 5   
   " ��6   .�5      ��  � ��$ 26(�6)   �� 5       � ��
 �����6+   �� 5       � ��$ Italic$�6*   �� 5       � ��
 �����6+   5      �� 5   
   " ��6   .�5      ��  � ��$
 Bold"�6+   5      �� 5   
   " ��6   .�5      ��  � ��
 �   �6,   5      5      .��5      ]� .+�5   
   �� $	��Top=�6   $��New Customer��6&   $��
ExtraLight"�6=   $��Center=�6U   �� 5   
    �� Orientation$ Vertical=�6   .+�5      ]� .+�5       �� $��Customer Code��6   $
��Left=�60   �� 5!      -   5         ��Name$ codeExpander��5!      |�.  $
��Left=�6A    � ToolTip$ Formatting Details��6W   $��40��6t   $
��Left=�6   
������ .��5"      �� -   5        $��codePopArrow��5"      $��/Art/prevNext.png��60   $��30��6K    J� RenderTransformOrigin$ 0.5,0.5&�6W   5#      � 5$      .+�5%      �� -   5        $��codePop��5%      $y�Hidden=�6,   $+��%Up to 100 characters. Must be unique.��6@   $��Center=�6m   	 �� TextAlignment$
	 Left=�6�   
 �� Padding$

 20 0q�6�   $��
UltraLight"�6�   $��20(�6�   �� 5&      -   5        $��codeErrorPop��5&   !   $
��Wrap=�65   $y�Hidden=�6I   $��This shouldn't be visible��6]   $��Center=�6~   $
	 Left=�6�   $

 20 0q�6�   
���<��$��22(�6�   5'      5(      �� 5)      $��4q�6   ]� 5*      .+�5+      �� -   5        $��codeValidationBtn��5+      |�.  $��Right=�6D   $ See Validation Errors��6[   .��5,      � .+�5-      �� -   5        $��codeAlertImg��5-       $y�Hidden=�64   $��/Art/alert.png��6H   $��30��6`   �� 5.      -   5         ��Name$ codeCheckImg��5.       $y�Hidden=�66   $��/Art/check.png��6J   $��30��6b   5/      50      �� 52      -	   5        $��custCreateCodeBox��52      $��Enter Customer Code Here��6/   $��26(�6O   6�   53      �� 54      $��10q�6   ]� 59      .+�5:      �� $
��Name��6   $
��Left=�6'   �� 5;      -
   5        $ nameExpander��5;      |�.  $
��Left=�6A   $ Formatting Details��6W   $��40��6t   $
��Left=�6   
������ .��5<      �� -   5        $��namePopArrow��5<      $��/Art/prevNext.png��60   $��30��6K   $ 0.5,0.5&�6W   5=      � 5>      .+�5?      �� -   5        $��namePop��5?      $y�Hidden=�6,   $K��ETwo words starting with capital letters, no more than 100 characters.��6@   $��Center=�6�   $
	 Left=�6�   $

 20 0q�6�   $��
UltraLight"�6�   $��20(�6�   �� 5@      -   5        $��nameErrorPop��5@   !   $
��Wrap=�65   $y�Hidden=�6I   $��This shouldn't be visible��6]   $��Center=�6~   $
	 Left=�6�   $

 20 0q�6�   
���<��$��22(�6�   5A      5B      �� 5C      $��4q�6   ]� 5D      .+�5E      �� -   5        $��nameValidationBtn��5E      |�.  $��Right=�6D   $ See Validation Errors��6[   .��5F      � .+�5G      �� -   5        $��nameAlertImg��5G       $y�Hidden=�64   $��/Art/alert.png��6H   $��30��6`   �� 5H      -   5        $ nameCheckImg��5H       $y�Hidden=�66   $��/Art/check.png��6J   $��30��6b   5I      5J      �� 5L      -   5        $��custCreateNameBox��5L   3   $��Enter Name Here��6L   6�   5M      �� 5O      $��10q�6   ]� 5T      .+�5U      �� $	��CNP��6   $
��Left=�6&   �� 5V      -   5        $ cnpExpander��5V      |�.  $
��Left=�6@   $ Formatting Details��6V   $��40��6s   $
��Left=�6~   
������ .��5W      �� -   5        $��cnpPopArrow��5W      $��/Art/prevNext.png��6/   $��30��6J   $ 0.5,0.5&�6V   5X      � 5Y      .+�5Z      �� -   5        $��cnpPop��5Z      $y�Hidden=�6+   $;��5Standard CNP: 13 Numbers (Gender)-(BirthDate)-(Code).��6?   $��Center=�6|   $
	 Left=�6�   $

 20 0q�6�   $��
UltraLight"�6�   $��20(�6�   �� 5[      -   5        $��cnpErrorPop��5[   !   $
��Wrap=�64   $y�Hidden=�6H   $��This shouldn't be visible��6\   $��Center=�6}   $
	 Left=�6�   $

 20 0q�6�   
���<��$��22(�6�   5\      5]      �� 5^      $��4q�6   ]� 5_      .+�5`      �� -   5        $��cnpValidationBtn��5`      |�.  $��Right=�6C   $ See Validation Errors��6Z   .��5a      � .+�5b      �� -   5        $��cnpAlertImg��5b       $y�Hidden=�63   $��/Art/alert.png��6G   $��30��6_   �� 5c      -   5        $ cnpCheckImg��5c       $y�Hidden=�65   $��/Art/check.png��6I   $��30��6a   5d      5e      �� 5f      -   5        $��custCreateCnpBox��5f   3   $��Enter CNP Here��6v   6�   5g      �� 5h      $��10q�6   ]� 5n      .+�5o      �� $��Address��6   $
��Left=�6*   �� 5p      -   5        $ adrExpander��5p      |�.  $
��Left=�6@   $ Formatting Details��6V   $��40��6s   $
��Left=�6~   
������ .��5q      �� -   5        $��adrPopArrow��5q      $��/Art/prevNext.png��6/   $��30��6J   $ 0.5,0.5&�6V   5r      � 5s      .+�5t      �� -   5        $��adrPop��5t      $y�Hidden=�6+   $"��No more than 200 characters.��6?   $��Center=�6c   $
	 Left=�6~   $

 20 0q�6�   $��
UltraLight"�6�   $��20(�6�   �� 5u      -   5        $��adrErrorPop��5u   !   $
��Wrap=�64   $y�Hidden=�6H   $��This shouldn't be visible��6\   $��Center=�6}   $
	 Left=�6�   $

 20 0q�6�   
���<��$��22(�6�   5v      5w      �� 5x      $��4q�6   ]� 5y      .+�5z      �� -   5        $��adrValidationBtn��5z      |�.  $��Right=�6C   $ See Validation Errors��6Z   .��5{      � .+�5|      �� -   5        $��adrAlertImg��5|       $y�Hidden=�63   $��/Art/alert.png��6G   $��30��6_   �� 5}      -    5        $ adrCheckImg��5}       $y�Hidden=�65   $��/Art/check.png��6I   $��30��6a   5~      5      �� 5�      -!   5        $��custCreateAdrBox��5�      $��Enter Address Here��6v   6�   5�      ]� 5�      .+�5�      �� -"   5        $��custCreateSubmitBtn��5�      $
��Left=�60   $��Submit��6F   $��20 20q�6W   $
��Left=�6f   $��40(�6�   $��DemiBold"�6�   $��20q�6�   
����� �6�   �� 5�      $	��300��6   �� 5�      -#   5        $ custCreateDiscardBtn��5�      
������ .��5�      �� $ 
Horizontal=�6!   .+�5�      �� $��/Art/cancel.png��6    $��70��69   �� 5�      $��Consolas)�6    $��Discard��66   $��38(�6H   $��	ExtraBold"�6V   
������ $��Center=�6�   5�      5�      5�      5�      5�      5�      