print('Start #################################################################');

db = db.getSiblingDB('UPV');
db.createUser(
  {
    user: 'init',
    pwd: 'init',
    roles: [{ role: 'readWrite', db: 'UPV' }],
  },
);
db.createCollection('users');

print('END #################################################################');