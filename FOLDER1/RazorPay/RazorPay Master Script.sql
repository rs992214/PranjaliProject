
GO
/****** Object:  Table [dbo].[Razor_Credential_Master]    Script Date: 10-Jun-21 3:41:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Razor_Credential_Master](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Razor_Test_Key] [nvarchar](50) NULL,
	[Razor_Test_Secrete] [nvarchar](50) NULL,
	[Razor_Test_Account] [nvarchar](50) NULL,
	[Razor_Live_Key] [nvarchar](50) NULL,
	[Razor_Live_Secrete] [nvarchar](50) NULL,
	[Razor_Live_Account] [nvarchar](50) NULL,
	[Razor_Status] [nvarchar](50) NULL,
	[Created_Date] [datetime] NULL,
	[Updated_Date] [datetime] NULL
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Razor_Credential_Master] ON 
GO
INSERT [dbo].[Razor_Credential_Master] ([ID], [Razor_Test_Key], [Razor_Test_Secrete], [Razor_Test_Account], [Razor_Live_Key], [Razor_Live_Secrete], [Razor_Live_Account], [Razor_Status], [Created_Date], [Updated_Date]) VALUES (1, N'rzp_test_Kpbhv00EJjZLkA', N'Lm7mdp3gHZpu3Ot80LeF3RqP', N'2323230050857322', N'rzp_test_Kpbhv00EJjZLkA', N'Lm7mdp3gHZpu3Ot80LeF3RqP', N'2323230050857322', N'Test', CAST(N'2021-03-19T12:32:53.590' AS DateTime), CAST(N'2021-04-07T16:34:20.413' AS DateTime))
GO
SET IDENTITY_INSERT [dbo].[Razor_Credential_Master] OFF
GO
