   M S B A M L   `   `   `  ���� J  FCleanMyComputer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:Nettoyeur	Nettoyeur      Nettoyeur.MainWindow   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     )localclr-namespace:Nettoyeur     -     �Name$  appName�� � Title$ 	Nettoyeur��$	��700��$
��1000�� � Icon$ /Laravel.png�� � 
ResizeMode$ NoResize=�.��� .+��� $��Center=�$	��165��
���   �$	��Top=�$
��1000�����	#FF007791�� -    ��Name$ Titre��$��Analyse du PC requise��$
��Left=�$��	30,20,0,0q�$	��Top=�$	��531��$��45��
�������$��	Aria Nova)�$��30(�$
��Bold"��� $��Espaces à nettoyer :��$
��Left=�$��	30,70,0,0q�$	��Top=�$	��207��$��36��
�������$��	Aria Nova)�$��20(�$
��Bold"��� $��Dernière analyse :��$
��Left=�$��
30,100,0,0q�$	��Top=�$	��178��$��33��
�������$��	Aria Nova)�$��20(�$
��Bold"��� -   $ espace��$��Inconnu��$
��Left=�$��
228,70,0,0q�$	��Top=�$	��411��$��36��
�������$��	Aria Nova)�$��20(��� -   $
 date��$��Jamais��$
��Left=�$��213,100,0,0q�$	��Top=�$	��369��$��33��
�������$��	Aria Nova)�$��20(��� -   $��Analyser��$
��Left=�$��
650,50,0,0q�$	��Top=�$	��250��$��70��
�������$��	Aria Nova)�$��30(�$
��Bold"����	#FF007991$��4,4,4,4q�
��������� $��Center=�$��13��$��	0,166,0,0q�$	��Top=�$
��1000�����	#FFD9E9EC�� -    ��Name$ btnClean��$
��Left=�$��
80,209,0,0q�$	��Top=�$	��350��$	��140��
�������$��	Aria Nova)�$��20(�$
��Bold"����	#FF007991$��4,4,4,4q�
��������� -   $ btnCleanMessage��$��	Nettoyage��$��Center=�$
��Left=�$��
80,297,0,0q�$	��Top=�$	��350��$��37��
�������$��	Aria Nova)�$��20(�$
��Bold"� J� IsHitTestVisible$ False���� -   $	 maj��$��


Mise à jour��$
��Left=�$��
80,384,0,0q�$	��Top=�$	��350��$	��140��
�������$��	Aria Nova)�$��20(�$
��Bold"����	#FF007991$��4,4,4,4q�
��������� -	   $ 
historique��$��


Historique��$
��Left=�$��570,209,0,0q�$	��Top=�$	��350��$	��140��
�������$��	Aria Nova)�$��20(�$
��Bold"����	#FF007991$��4,4,4,4q�
��������� -
   $
 more��$��


En savoir plus��$
��Left=�$��570,384,0,0q�$	��Top=�$	��350��$	��140��
�������$��	Aria Nova)�$��20(�$
��Bold"����	#FF007991$��4,4,4,4q�
��������� $��Center=�$	��134��$��	0,550,0,0q�$	��Top=�$
��1000�����	#FF86D9EC�� -   $ 	copyright��$��Copyright 2022 - Rijenth��$��Center=�$��Center=�$��	0,573,0,0q�$	��Top=�$	��960��$��44��
���   �$��	Aria Nova)�$��25(��� -    ��Name$ bandeau��$��Center=�$��56��$��	0,628,0,0q�$	��Top=�$
��1000�����	#FF36ABC5$y�Hidden=��� -   $
 actu��$
��2022��$��Center=�$��Center=�$��	0,628,0,0q�$	��Top=�$	��960��$��44��
���   �$��	Aria Nova)�$��20(�$
��Bold"�$y�Hidden=��� $
��Left=�$��60��$��224,230,0,0q�$	��Top=�$��60��$��
/loupe.png��$ False���� $
��Left=�$��60��$��715,230,0,0q�$	��Top=�$��60��$��/historique.png��$ False���� $
��Left=�$��60��$��224,408,0,0q�$	��Top=�$��60��$��/miseajour.png��$ False���� $
��Left=�$��60��$��715,408,0,0q�$	��Top=�$��60��$��/information.png��$ False��