   M S B A M L   `   `   `  ����T  PCustomerWPF_DesktopClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullc6clr-namespace:CustomerWPF_DesktopClient.Views.SubViews(CustomerWPF_DesktopClient.Views.SubViews  5   	   A    ;CustomerWPF_DesktopClient.Views.SubViews.CustomerUpdateView   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   Hlocal6clr-namespace:CustomerWPF_DesktopClient.Views.SubViews     5   	   -   5          � Title$  Update a Customer��5      $	��540��6   $	��800��6,   $��30(�68    � 	Resources 5
       �� 
TargetType �� Property �� Value��%�  '��       '�� �     '�� 0    '�� z    �� 5   
   " ��6   .�5      ��  � ��$ 26(�6)   �� 5       � ��
 �����6+   �� 5       � ��$ Italic$�6*   �� 5       � ��
 �����6+   5      �� 5   
   " ��6   .�5      ��  � ��$ 
UltraLight"�6+   �� 5       � ��$ 18(�6)   5      �� 5   
   " ��6   .�5      ��  � ��$
 Bold"�6+   5      �� 5   
   " ��6   .�5      ��  � ��
 �   �6,   5      5      .��5      ]� .+�5   
   �� $	��Top=�6   $��Update Customer��6&   $��
ExtraLight"�6@   $��Center=�6X   �� 5   
    �� Orientation$ Vertical=�6   .+�5#      ]� .+�5$      �� $
��Name��6   $
��Left=�6'   �� 5%      -   5         ��Name$ nameExpander��5%      |�.  $
��Left=�6A    � ToolTip$ Formatting Details��6W   $��40��6t   $
��Left=�6   
������ .��5&      �� -   5        $��namePopArrow��5&      $��/Art/prevNext.png��60   $��30��6K    J� RenderTransformOrigin$ 0.5,0.5&�6W   5'      � 5(      .+�5)      �� -   5        $��namePop��5)      $y�Hidden=�6,   $K��ETwo words starting with capital letters, no more than 100 characters.��6@   $��Center=�6�   	 �� TextAlignment$
	 Left=�6�   
 �� Padding$

 20 0q�6�   $��
UltraLight"�6�   $��20(�6�   �� 5*      -   5        $��nameErrorPop��5*      $
��Wrap=�61   $y�Hidden=�6E   $��This shouldn't be visible��6Y   $��Center=�6z   $
	 Left=�6�   $

 20 0q�6�   
���<��$��22(�6�   5+      5,      �� 5-      $��4q�6   ]� 5.      .+�5/      �� -   5        $��nameResetBtn��5/      |�.  $��Right=�6?   
������ $ Reset to original Name��6�   .��50      �� $��/Art/reset.png��6   $��40��64   $��50��6@   51      �� 52      -   5        $��nameValidationBtn��52      |�.  $��Right=�6D   $ See Validation Errors��6[   .��53      � .+�54      �� -   5        $��nameAlertImg��54      $y�Hidden=�60   $��/Art/alert.png��6D   $��30��6\   �� 55      -	   5         ��Name$ nameCheckImg��55      $y�Hidden=�62   $��/Art/check.png��6F   $��30��6^   56      57      �� 58      -
   5        $��custUpdNameBox��58      $��Enter Name Here��6I   6�   59      �� 5:      $��10q�6   ]� 5@      .+�5A      �� $	��CNP��6   $
��Left=�6&   �� 5B      -   5        $ cnpExpander��5B      |�.  $
��Left=�6@   $ Formatting Details��6V   $��40��6s   $
��Left=�6~   
������ .��5C      �� -   5        $��cnpPopArrow��5C      $��/Art/prevNext.png��6/   $��30��6J   $ 0.5,0.5&�6V   5D      � 5E      .+�5F      �� -   5        $��cnpPop��5F   !   $y�Hidden=�6/   $;��5Standard CNP: 13 Numbers (Gender)-(BirthDate)-(Code).��6C   $��Center=�6�   $
	 Left=�6�   $

 20 0q�6�   $��
UltraLight"�6�   $��20(�6�   �� 5G      -   5         ��Name$ cnpErrorPop��5G       $y�Hidden=�65   $%��Should not be able to see this.��6I   $��Center=�6p   $
	 Left=�5H       $

 20 0q�65   
���<��$��22(�6Y   $
��Wrap=�6g   5I      5J      �� 5K      $��4q�6   ]� 5L      .+�5M      �� -   5        $��cnpResetBtn��5M      |�.  $��Right=�6>   
������ $ Reset to original CNP��6�   .��5N      �� $��/Art/reset.png��6   $��40��64   $��50��6@   5O      �� 5P      -   5        $��cnpValidationBtn��5P      |�.  $��Right=�6C   $ See Validation Errors��6Z   .��5Q      � .+�5R      �� -   5        $��cnpAlertImg��5R       $y�Hidden=�63   $��/Art/alert.png��6G   $��30��6_   �� 5S      -   5        $ cnpCheckImg��5S       $y�Hidden=�65   $��/Art/check.png��6I   $��30��6a   5T      5U      �� 5V      -   5        $��custUpdCnpBox��5V      $��Enter CNP Here��6/   6�   5W      �� 5X      $��10q�6   ]� 5^      .+�5_      �� $��Address��6   $
��Left=�6*   �� 5`      -   5        $ adrExpander��5`      |�.  $
��Left=�6@   $ Formatting Details��6V   $��40��6s   $
��Left=�6~   
������ .��5a      �� -   5        $��adrPopArrow��5a      $��/Art/prevNext.png��6/   $��30��6J   $ 0.5,0.5&�6V   5b      � 5c      .+�5d      �� -   5        $��adrPop��5d      $y�Hidden=�6+   $"��No more than 200 characters.��6?   $��Center=�6c   $
	 Left=�6~   $

 20 0q�6�   $��
UltraLight"�6�   $��20(�6�   �� 5e      -   5        $��adrErrorPop��5e   !   $y�Hidden=�64   $$��Should not be able to see this��6H   $��Center=�6n   $
	 Left=�6�   $

 20 0q�6�   
���<��$��22(�6�   $
��Wrap=�6�   5f      5g      �� 5h      $��4q�6   ]� 5i      .+�5j      �� -   5        $��adrResetBtn��5j      |�.  $��Right=�6>   
������ $ Reset to original Address��6�   .��5k      �� $��/Art/reset.png��6   $��40��64   $��50��6@   5l      �� 5m      -   5        $��adrValidationBtn��5m      |�.  $��Right=�6C   $ See Validation Errors��6Z   .��5n      � .+�5o      �� -   5        $��adrAlertImg��5o       $y�Hidden=�63   $��/Art/alert.png��6G   $��30��6_   �� 5p      -   5        $ adrCheckImg��5p       $y�Hidden=�65   $��/Art/check.png��6I   $��30��6a   5q      5r      �� 5s      -   5        $��custUpdAdrBox��5s   3   $��Enter Address Here��6H   6�   5t      ]� 5z      .+�5|      �� -   5        $��custUpdSubmitBtn��5|      $
��Left=�61   $��Submit��6G   $��20 20q�6X   $
��Left=�6g   $��40(�6�   $��DemiBold"�6�   $��20q�6�   
����� �$ Commit Update (Enter)��6�   �� 5}      $	��300��6   �� 5      $ Vertical=�6   $��Center=�64   .+�5�      �� -   5        $ custUpdDiscardBtn��5�      
������ $@ :Discard changes and return to main Customers View (Escape)��6q   .��5�      �� $ 
Horizontal=�6%   .+�5�      �� $��/Art/cancel.png��6$   $��60��6=   �� 5�      $��Consolas)�6$   $��Discard��6:   $��30(�6L   $��	ExtraBold"�6Z   
������ $��Center=�6�   5�      5�      �� 5�      -   5        $ custUpdResetAllBtn��5�      
������ $; 5Restore original values for all fields (Ctrl+Shift+R)��6�   .��5�      �� $ 
Horizontal=�6%   .+�5�      �� $��/Art/reset.png��6$   $��50��6<   �� 5�      $��Consolas)�6$   $��	Reset All��6:   $��30(�6N   $��	ExtraBold"�6\   
������ $��Center=�6�   5�      5�      5�      5�      5�      5�      5�      