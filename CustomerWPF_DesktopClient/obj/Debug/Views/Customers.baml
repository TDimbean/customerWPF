   M S B A M L   `   `   `  ����  CustomerWPF_DesktopClient/    )CustomerWPF_DesktopClient.Views.Customers   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      -   5          � Title$  	Customers��5	       � 	Resources 5      =  7System.Windows.Controls.Primitives.DataGridColumnHeader �� 
TargetType �� Property �� Value*  $System.Windows.Controls.DataGridCell��%�   '        '  p     �� 5   
   "  6   .�5      ��  � ��$ 20(�6)   �� 5       � ��$ Center=�6;   5      �� 5   
   "  6   .�5      ��  � ��$ 18(�6)   5      5      .��5      ]�  +� FocusedElement 6   ��  �� ElementName$ customerSearchBox��.+�5   
   ]� $��60��6   $	��Top=�6    .+�5      �� -   5         ��Name$ backToMainBtn��5   +   $
��Left=�6   
������  � ToolTip$/ )Go back to main window.(Escape/Home/Back)��6w   .��5      �� $��/Art/back.png��6   $��50��6/   5      �� 5      -   5        	 ��Name$	 customerSearchBox��5   E   $
��Left=�6   $��
UltraLight"�6`   
�������$��Italic$�6�   $��	Search...��6�   $��28(�6�   $��10q�6
  
�������$	��650��6/  $	��650��6;  6J  �� 5      -   5        $��customerSearchBtn��5   ]   $
��Left=�6   
������ $ Search(Enter)��6�   .��5       �� $��/Art/search.png��6   $��50��61   5!      �� 5"      $
��Left=�6   
����� �6.   �� 5#      -   5        $��customerToCardsBtn��5#   a   $��Right=�6   $��Right=�6,   
������ $ Cards View(,/.)��6�   .��5$      � .+�5%      �� $��/Art/cardsSimple.png��6   $��50��6:   �� 5&      $��Cards��6   $��Center=�6,   $��20(�6G   5'      5(      �� 5)      $��Right=�6   
����� �$
��auto��6B   $��10q�6O   �� 5*      -   5        $��CardsResetBtn��5*   a   $��Right=�6   $��Right=�6,   
������ $ Reset View (*)��6�   .��5+      �� $��/Art/reset.png��6   $��50��60   5,      5-      ]� 53   
   $	��Top=�6   $	��0 4q�6)   
�������.+�54      �� 
 �� Orientation$
 
Horizontal=�6   $
��Left=�62   .+�55      �� $��	Showing: ��6   $��20(�6(   �� 56      -   5        $"	 custViewResultsPerPageTxtBox��56   3   
������ $��50��6r   $��Center=�6}   $��20(�6�   $
��Bold"�6�    �� TextAlignment$ Center=�6�   $	��All��6�   69  �� 57      -   5        $��custPerPageLabel��57   n   $��Center=�6   $��20(�6L   $y�Hidden=�6Z   6n   58      �� 59      $
 
Horizontal=�6   $��Right=�62   $��Right=�6I   .+�5:      �� -	   5        $��custPrevBtn��5:      
������ .��5;      �� $��/Art/prevNext.png��6   $��30��67    J� RenderTransformOrigin$ 0.5 0.5&�6C   z�5<      �� 5=       �� ScaleX$ -1Q�6-   5?      5@      �� 5A      $��Page: ��6   $��20(�6)   �� 5B      -
   5         ��Name$ custPageIndexTxtBlock��5B      $��1��6T   $
��Bold"�6^   $��Center=�6p   $��20(�6�   �� 5C      $��of��6   $��20(�6%   �� 5D      -   5        $ custPageCountTxtBlock��5D      $��1��6T   $
��Bold"�6^   $��Center=�6p   $��20(�6�   �� 5E      -   5        $��custNextBtn��5E      
������ .��5F      �� $��/Art/prevNext.png��6   $��30��67   5G      5H      5I      &   System.Windows.Controls.DataGrid  5O   
   -   5          Name$ customersDataGrid��5O         AutoGenerateColumns .     CanUserAddRows .     ColumnWidth5  /System.Windows.Controls.DataGridLengthConverter$ * 6z      SelectionMode$ Single=�6�      SelectionUnit$ FullRow=�6�      
IsReadOnly .    Columns 5P      0  *System.Windows.Controls.DataGridTextColumn  5Q      ,  &System.Windows.Controls.DataGridColumn   Header$ Customer Code��6%      
IsReadOnly . 1  +System.Windows.Controls.DataGridBoundColumn   Binding 6<   �� *CustomerCode+6]     5R      $
 Name��6%    63   �� *Name+  5S      $	 CNP��6%    62   �� *CNP+  5T      $ Address��6%    66   �� *	Address+4  .System.Windows.Controls.DataGridTemplateColumn  5U         Width$ 50 6)      HeaderTemplate 5V      �� 5W      .R�5X      �� $ custViewAddButton��6%   -   5        
������ $ New Customer(+)��5X   y   .��5Y   "   �� $��/Art/plus.png��6(   $��50��6?   5Z      5[         CellTemplate 5]      �� 5^      .R�5_      �� $ custViewUpdateButton��6%   -   5        $ Update��5_   f   .��5`   "   �� $��/Art/update.png��6(   $��40��6A   5a      5b      5d      5h      5l      5m      