# How to set up:
**DO NOT RENAME FILES!**
[Google Drive Resources Link](https://drive.google.com/drive/folders/1RrKFhvpfSOd_NvZDBaOigv3vfshP8UG8?usp=sharing)
[Google Drive Documentation Link](https://docs.google.com/document/d/1HQApeNb_5khwMVWohOqjiFUCEU5NX-4kRssoVjRMGxs?usp=sharing)

1. Unzip **nnlm.zip** to **./Classify**, **./SearchForQuery** AND to **./TagsAppender
2. Unzip **stanford-server.zip** to **./Classify** AND to **./TagsAppender
3. Unzip **UPV.zip** to **./Mongo/docker-entrypoint-initdb.d**
4. Unzip **initdbfull.zip** to **./MSSQL/mssql_database**
5. Install both dependencies from **./Classify/requirements.txt** AND **./SearchForQuery/requirements.txt**

# How to launch Docker:
1. Open command prompt in the folder where the repository is
	- Each folder has a docker-compose, you can use those to make a cluster for each component with **docker compose up**
	- **docker compose -f docker-compose.full.yml up** to make a single cluster