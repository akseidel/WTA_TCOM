   M S B A M L   `   `   `  ����   WTA_TCOM    WTA_TCOM.WPF_TCOMSettings   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     -     � SizeToContent$  Height=�$	��150��$	��330��$	��600��$��75��
 � Top$	 200�� � Left$	 600�� � Topmost .  � WindowStyle$
 None=�
������  � AllowsTransparency . $��0q� � 	Resources  �� 
TargetType �� Property	 �� Value��%D   '_�       �� " _�.���  � ��
	 ���� ��  � ��$	 0q�.���� -   $
��Body��
�������
 �� CornerRadius$
 24��$��3q�
�������._�]� ��. $��4q�.+��� -    ��Name$ TopLine��$	��Top=� �� TextAlignment$ Center=�$��semiBold"�$��14(�$	��4,2q�$��TCOM Settings���� -    ��Name$
 Quit��$
��Left=�
������ 
������  �� IsCancel . $��Center=��� -   $ BotLine��$��Bottom=�$ Center=�$��semiBold"�$��12(�$��4,2,4,0q�$)��#Press ESC or click here to dismiss.���� -    ��Name$ MainSettings�� �� Orientation$ Vertical=�.+�&   System.Windows.Controls.DataGrid  -   $��TabsControlGrid��$��14(�   ColumnWidth5  /System.Windows.Controls.DataGridLengthConverter$ *    RowHeaderWidth$ 0Q�   CanUserAddRows .     CanUserDeleteRows .     AlternatingRowBackground
 �����   HeadersVisibility$ Column=�   SelectionUnit$ FullRow=�   SelectionMode$ Extended=�   CanUserSortColumns .  $�� 0, 0, 0, 6q�   AutoGenerateColumns .     Columns 0  *System.Windows.Controls.DataGridTextColumn  ,  &System.Windows.Controls.DataGridColumn   
IsReadOnly .    Header$ Setting Name��   Width$ *    CanUserResize .  1  +System.Windows.Controls.DataGridBoundColumn    Binding  �� ! �� Path$! 	MySetName�" �� Mode$" TwoWay=�# �� UpdateSourceTrigger$# PropertyChanged=�4  .System.Windows.Controls.DataGridTemplateColumn  $ Visble��$
 Auto  .  $   CellTemplate$ �� .R��� $��Center=�$��Center=�% d� 	IsChecked% �� $! MyOnOffSetBool�$" TwoWay=�$# PropertyChanged=��� -   $ spRootSearchPath��$ 
Horizontal=�.+��� $��Family Root Search Path:��$��14(��� -	   & ��Name$& RootSearchPath��$��14(�$��Stretch=�$��What Me Worry��$	��4,0q�$	��143��.��' � ToolTip' �� $
 10��
�������$	��310��$��3q�
�������._��� $��10q�$��14(�$ Justify=�$��Stretch=�,*When a family is not already loaded in the�� +)project, this is the starting folder from�� -+which all child folders will be searched to�� find that family to load..��