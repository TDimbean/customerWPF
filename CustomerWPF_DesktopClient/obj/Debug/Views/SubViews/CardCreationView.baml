   M S B A M L   `   `   `  ����T  PCustomerWPF_DesktopClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullc6clr-namespace:CustomerWPF_DesktopClient.Views.SubViews(CustomerWPF_DesktopClient.Views.SubViews  5   	   ?    9CustomerWPF_DesktopClient.Views.SubViews.CardCreationView   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   Hlocal6clr-namespace:CustomerWPF_DesktopClient.Views.SubViews     5   	   -   5          � Title$  Add a New Card��5      $	��800��6   $	��800��6%   $��30(�61    � 	Resources 5
       �� 
TargetType �� Property �� Value(  "System.Windows.Controls.DatePicker��%  '��       '  �     '�� �    '�� U    '�� �    �� 5   
   " ��6   .�5      ��  � ��$ 26(�6)   �� 5       � ��
 �����6+   �� 5       � ��
 �����6+   �� 5       � ��$ Italic$�6*   5      �� 5   
   "  6   .�5      ��  � ��$	 160��6&   �� 5       � ��$ 17(�6)   �� 5       �@ FirstDayOfWeek$ Monday=�6/   �� 5       � |� .  6*   �� 5       � ��$ Consolas)�6+   �� 5       �@ IsTodayHighlighted .  63   5      �� 5   
   " ��6   .�5      ��  � ��$ 
UltraLight"�6+   �� 5       � ��$ 18(�6)   5      �� 5   
   " ��6   .�5      ��  � ��$
 Bold"�6+   5      �� 5    
   " ��6   .�5!      ��  � ��
 �   �6,   5"      5$      .��5'      ]� .+�5(   
   �� $	��Top=�6   $��New Card��6&   $��
ExtraLight"�69   $��Center=�6Q   �� 5)   
    �� Orientation$ Vertical=�6   .+�5-      ]� .+�5.      �� $��	Card Code��6   $
��Left=�6,   �� 5/      -   5         ��Name$ codeExpander��5/      |�.  $
��Left=�6A    � ToolTip$ Formatting Details��6W   $��40��6t   $
��Left=�6   
������ .��50      �� -   5        $��codePopArrow��50      $��/Art/prevNext.png��60   $��30��6K    J� RenderTransformOrigin$ 0.5,0.5&�6W   51      � 52      .+�53      �� -   5        $��codePop��53      $y�Hidden=�6,   $)��#Up to 15 characters. Must be unique��6@   $��Center=�6k   	 �� TextAlignment$
	 Left=�6�   
 �� Padding$

 20 0q�6�   $��
UltraLight"�6�   $��20(�6�   �� 54      -   5        $��codeErrorPop��54      $
��Wrap=�61   $y�Hidden=�6E   $��This shouldn't be visible��6Y   $��Center=�6z   $
	 Left=�6�   $

 20 0q�6�   
���<��$��22(�6�   55      56      �� 57      $��4q�6   ]� 58      .+�59      �� -   5        $��codeValidationBtn��59      |�.  $��Right=�6D   $ See Validation Errors��6[   .��5:      � .+�5;      �� -   5        $��codeAlertImg��5;       $y�Hidden=�64   $��/Art/alert.png��6H   $��30��6`   �� 5<      -   5         ��Name$ codeCheckImg��5<       $y�Hidden=�66   $��/Art/check.png��6J   $��30��6b   5=      5>      �� 5?      -	   5        $��cardCreateCodeBox��5?      $��Enter Card Code Here��6/   $��26(�6d   6�   5@      �� 5A      $��10q�6   ]� 5G      .+�5H      �� $��Unique Number��6   $
��Left=�60   �� 5I      -
   5        $ 
unExpander��5I      $
��Left=�6-   $ Formatting Details��6C   $��40��6`   $
��Left=�6k   
������ |�.  .��5J      �� -   5        $��
unPopArrow��5J      $��/Art/prevNext.png��6.   $��30��6I   $ 0.5,0.5&�6U   5K      � 5L      .+�5M      �� -   5        $��unPop��5M   !   $y�Hidden=�6.   $7��150 character maximum, the first 4 must be digits.��6B   $��Center=�6{   $
	 Left=�6�   $

 20 0q�6�   $��
UltraLight"�6�   $��20(�6�   �� 5N      -   5        $��
unErrorPop��5N   !   $
��Wrap=�63   $y�Hidden=�6G   $��This shouldn't be visible��6[   $��Center=�6|   $
	 Left=�6�   $

 20 0q�6�   
���<��$��22(�6�   5O      5P      �� 5Q      $��4q�6   ]� 5R      .+�5S      �� -   5        $��unValidationBtn��5S      |�.  $��Right=�6B   $ See Validation Errors��6Y   .��5T      � .+�5U      �� -   5        $��
unAlertImg��5U       $y�Hidden=�62   $��/Art/alert.png��6F   $��30��6^   �� 5V      -   5        $ 
unCheckImg��5V       $y�Hidden=�64   $��/Art/check.png��6H   $��30��6`   5W      5X      �� 5Y      -   5        $��cardCreateUnBox��5Y      $��Enter Unique Number Here��61   6�   5Z      �� 5[      $��10q�6   ]� 5a      .+�5b      �� $��
CVV Number��6   $
��Left=�6-   �� 5c      -   5        $ cvvExpander��5c      |�.  $
��Left=�6@   $ Formatting Details��6V   $��40��6s   $
��Left=�6~   
������ .��5d      �� -   5        $��cvvPopArrow��5d      $��/Art/prevNext.png��6/   $��30��6J   $ 0.5,0.5&�6V   5e      � 5f      .+�5g      �� -   5        $��cvvPop��5g   !   $y�Hidden=�6/   $7��115 character maximum, the first 8 must be digits.��6C   $��Center=�6|   $
	 Left=�6�   $

 20 0q�6�   $��
UltraLight"�6�   $��20(�6�   �� 5h      -   5        $��cvvErrorPop��5h   !   $
��Wrap=�64   $y�Hidden=�6H   $��This shouldn't be visible��6\   $��Center=�6}   $
	 Left=�6�   $

 20 0q�6�   
���<��$��22(�6�   5i      5j      �� 5k      $��4q�6   ]� 5l      .+�5m      �� -   5        $��cvvValidationBtn��5m      |�.  $��Right=�6C   $ See Validation Errors��6Z   .��5n      � .+�5o      �� -   5        $��cvvAlertImg��5o       $y�Hidden=�63   $��/Art/alert.png��6G   $��30��6_   �� 5p      -   5        $ cvvCheckImg��5p       $y�Hidden=�65   $��/Art/check.png��6I   $��30��6a   5q      5r      �� 5s      -   5        $��cardCreateCvvBox��5s      $��Enter CVV Here��6v   6�   5t      �� 5u      $��10q�6   ]� 5{      .+�5|      �� $��
Start Date��6   $
��Left=�6-   �� 5}      -   5        $ startExpander��5}      |�.  $
��Left=�6B   $ Formatting Details��6X   $��40��6u   $
��Left=�6�   
������ .��5~      �� -   5        $��startPopArrow��5~      $��/Art/prevNext.png��61   $��30��6L   $ 0.5,0.5&�6X   5      � 5�      .+�5�      �� -   5        $��startPop��5�   !   $y�Hidden=�61   $#��Up to 3 years beore End Date.��6E   $��Center=�6j   $
	 Left=�6�   $

 20 0q�6�   $��
UltraLight"�6�   $��20(�6�   �� 5�      -   5        $��startErrorPop��5�   !   $
��Wrap=�66   $y�Hidden=�6J   $��This shouldn't be visible��6^   $��Center=�6   $
	 Left=�6�   $

 20 0q�6�   
���<��$��22(�6�   5�      5�      �� 5�      $��4q�6   ]� 5�      .+�5�      �� -   5        $��startValidationBtn��5�      |�.  $��Right=�6E   $ See Validation Errors��6\   .��5�      � .+�5�      �� -   5        $��startAlertImg��5�       $y�Hidden=�65   $��/Art/alert.png��6I   $��30��6a   �� 5�      -    5        $ startCheckImg��5�       $y�Hidden=�67   $��/Art/check.png��6K   $��30��6c   5�      5�        5�      -!   5        $��startDatePicker��5�   4   $��Right=�6      SelectedDate$ 	2017/3/18X�6L      DisplayDate$ 	2017/3/18��6e      Text$ Select start date��6}   
���  ��6�   �� 5�      -"   5        $��cardCreateStartBox��5�      $��Enter Start Date Here��6z   6�   5�      �� 5�      $��10q�6   ]� 5�      .+�5�      �� $��End Date��6   $
��Left=�6+   �� 5�      -#   5        $ stopExpander��5�      |�.  $
��Left=�6A   $ Formatting Details��6W   $��40��6t   $
��Left=�6   
������ .��5�      �� -$   5        $��stopPopArrow��5�      $��/Art/prevNext.png��60   $��30��6K   $ 0.5,0.5&�6W   5�      � 5�      .+�5�      �� -%   5        $��stopPop��5�   !   $y�Hidden=�60   $%��Up to 3 years after Start Date.��6D   $��Center=�6k   $
	 Left=�6�   $

 20 0q�6�   $��
UltraLight"�6�   $��20(�6�   �� 5�      -&   5        $��stopErrorPop��5�   !   $
��Wrap=�65   $y�Hidden=�6I   $��This shouldn't be visible��6]   $��Center=�6~   $
	 Left=�6�   $

 20 0q�6�   
���<��$��22(�6�   5�      5�      �� 5�      $��4q�6   ]� 5�      .+�5�      �� -'   5        $��stopValidationBtn��5�      |�.  $��Right=�6D   $ See Validation Errors��6[   .��5�      � .+�5�      �� -(   5        $��stopAlertImg��5�       $y�Hidden=�64   $��/Art/alert.png��6H   $��30��6`   �� 5�      -)   5        $ stopCheckImg��5�       $y�Hidden=�66   $��/Art/check.png��6J   $��30��6b   5�      5�        5�      -*   5        $��endDatePicker��5�   4   $��Right=�6   $ 	2018/3/18X�6J   $ 	2018/3/18��6c   $ Select end date��6{   
���  ��6�   �� 5�      -+   5        $��cardCreateStopBox��5�      $��Enter End Date Here��6x   6�   5�      �� 5�      $��10q�6   ]� 5�      .+�5�      �� $��Customer Code��6   $
��Left=�60   �� 5�      -,   5        $ 
ccExpander��5�      |�.  $
��Left=�6?   $ Formatting Details��6U   $��40��6r   $
��Left=�6}   
������ .��5�      �� --   5        $��
ccPopArrow��5�      $��/Art/prevNext.png��6.   $��30��6I   $ 0.5,0.5&�6U   5�      � 5�      .+�5�      �� -.   5        $��ccPop��5�   !   $y�Hidden=�6.   $��Customer Code must exist.��6B   $��Center=�6c   $
	 Left=�6~   $

 20 0q�6�   $��
UltraLight"�6�   $��20(�6�   �� 5�      -/   5        $��
ccErrorPop��5�   !   $
��Wrap=�63   $y�Hidden=�6G   $��This shouldn't be visible��6[   $��Center=�6|   $
	 Left=�6�   $

 20 0q�6�   
���<��$��22(�6�   5�      5�      �� 5�      $��4q�6   ]� 5�      .+�5�      �� -0   5        $��ccValidationBtn��5�      |�.  $��Right=�6B   $ See Validation Errors��6Y   .��5�      � .+�5�      �� -1   5        $��
ccAlertImg��5�       $y�Hidden=�62   $��/Art/alert.png��6F   $��30��6^   �� 5�      -2   5        $ 
ccCheckImg��5�       $y�Hidden=�64   $��/Art/check.png��6H   $��30��6`   5�      5�      �� 5�      -3   5        $��cardCreateCcBox��5�      $��Enter Customer Code Here��6t   $��26(�6�   6�   5�      ]� 5�      .+�5�      �� -4   5        $��cardCreateSubmitBtn��5�   +   $
��Left=�6   $��Submit��6G   $��20 20q�6X   $
��Left=�6g   $��40(�6�   $��DemiBold"�6�   $��20q�6�   
����� �6�   �� 5�      $	��300��6   
������ 6$   �� 5�      -5   5        $��cardCreateDiscardBtn��5�   .   
������ .��5�      �� $ 
Horizontal=�6   $��Right=�66   .+�5�      �� $��/Art/cancel.png��6   $��70��65   �� 5�      $��Consolas)�6   $��Discard��62   $��38(�6D   $��	ExtraBold"�6R   
������ $��Center=�6�   5�      5�      5�      5�      5�      5�      