   M S B A M L   `   `   `  ����T  PCustomerWPF_DesktopClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullQ-clr-namespace:CustomerWPF_DesktopClient.ViewsCustomerWPF_DesktopClient.Views  5      +    %CustomerWPF_DesktopClient.Views.Cards   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      ?local-clr-namespace:CustomerWPF_DesktopClient.Views     5      -   5          � Title$  Cards��5	       � 	Resources 5      =  7System.Windows.Controls.Primitives.DataGridColumnHeader �� 
TargetType �� Property �� Value*  $System.Windows.Controls.DataGridCell��%�   '        '  p     �� 5   
   "  6   .�5      ��  � ��$ 20(�6)   �� 5       � ��$ Center=�6;   5      �� 5   
   "  6   .�5      ��  � ��$ 18(�6)   5      5      .��5      ]�  +� FocusedElement 6   ��  �� ElementName$ cardSearchBox��.+�5   
   ]� $��60��6   $	��Top=�6    .+�5      �� -   5         ��Name$ backToMainBtn��5   +   $
��Left=�6   
������  � ToolTip$/ )Go back to main window.(Escape/Home/Back)��6w   .��5      �� $��/Art/back.png��6   $��50��6/   5      �� 5      -   5        	 ��Name$	 cardSearchBox��5   E   $
��Left=�6   $��
UltraLight"�6�   $��Italic$�6�   $��	Search...��6�   $��28(�6�   $��10q�6  
�������$	��650��6-  $	��650��69  �� 5      -   5        $��cardSearchBtn��5   D   $
��Left=�6   
������ $ Search(Enter)��6�   .��5      �� $��/Art/search.png��6   $��50��61   5       �� 5!      
����� �6   �� 5"      -   5        $��cardsToCutomersBtn��5"   e   $��Right=�6   $��Right=�60   
������ $ Customers View(,/.)��6�   .��5#      � .+�5$      �� $��/Art/customer.png��6    $��50��6;   �� 5%      $��	Customers��6    $��Center=�64   $��20(�6O   5&      5'      �� 5(      $��Right=�6   
����� �$��10q�6F   �� 5)      -   5        $��CustomersResetFilters��5)   e   $��Right=�6   $��Right=�65   
������ $ Reset View (*)��6�   .��5*      �� $��/Art/reset.png��6   $��50��64   5+      5,      ]� 52   
   $	��Top=�6   $	��0 4q�6)   
�������.+�53      �� 
 �� Orientation$
 
Horizontal=�6   $
��Left=�62   .+�54      �� $��	Showing: ��6   $��20(�6,   �� 55      -   5        $	 cardsResultsPerPageTxtBox��55   L   
������ $��50��6o   $��Center=�6z   $��20(�6�   $
��Bold"�6�    �� TextAlignment$ Center=�6�   $	��All��6�   60  �� 56      -   5        $��cardPerPageLabel��56   o   $��Center=�6   $��20(�63   $y�Hidden=�6A   6o   57      �� 58      $
 
Horizontal=�6   $��Right=�62   $��Right=�6I   .+�59      �� -	   5        $��cardPrevBtn��59      
������ .��5:      �� $��/Art/prevNext.png��6   $��30��67    J� RenderTransformOrigin$ 0.5 0.5&�6C   z�5;      �� 5<       �� ScaleX$ -1Q�6-   5>      5?      �� 5@      $��Page: ��6   $��20(�6)   �� 5A      -
   5         ��Name$ cardPageIndexBlock��5A      $��1��6Q   $
��Bold"�6[   $��Center=�6m   $��20(�6�   �� 5B      $��of��6   $��20(�6%   �� 5C      -   5        $ cardPageCountBlock��5C      $��1��6Q   $
��Bold"�6[   $��Center=�6m   $��20(�6�   �� 5D      -   5        $��cardNextBtn��5D      
������ .��5E      �� $��/Art/prevNext.png��6   $��30��67   5F      5G      5H      &   System.Windows.Controls.DataGrid  5O   
   -   5          Name$ cardsDataGrid��5O         CanUserAddRows .     AutoGenerateColumns .     ColumnWidth5  /System.Windows.Controls.DataGridLengthConverter$ * 6v      SelectionMode$ Single=�5P         SelectionUnit$ FullRow=�6*      
IsReadOnly .    Columns 5Q      0  *System.Windows.Controls.DataGridTextColumn  5R      ,  &System.Windows.Controls.DataGridColumn   Header$ 	Card Code��6%      
IsReadOnly . 1  +System.Windows.Controls.DataGridBoundColumn   Binding 68   �� *
CardCode+6U     5S      $	 CVV��6%    62   �� *	CVVNumber+  5T      $ Unique Number��6%    6<   �� *UniqueNumber+  5U      $ 
Start Date��6%    69   �� *	StartDate+ �� StringFormat$ dd-MMM-yyyy��  5V      $ End Date��6%    67   �� *	EndDate+$ dd-MMM-yyyy��  5W      $ Customer Code��6%    6<   �� *CustomerCode+4  .System.Windows.Controls.DataGridTemplateColumn  5X         Width$ 50 6)      HeaderTemplate 5Y      �� 5Z      .R�5[      �� $ CardViewAddBtn��6%   -   5        
������ $ New Card(+)��5[   s   .��5\      �� $��/Art/plus.png��6$   $��50��6;   5]      5^         CellTemplate 5`      �� 5a      .R�5b      �� $ cardViewUpdateButton��6%   -   5        $ Update��5b   f   .��5c   "   �� $��/Art/update.png��6(   $��40��6A   5d      5e      5g      5i      5m      5n      