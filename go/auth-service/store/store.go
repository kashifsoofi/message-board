package store

type Store interface {
	Close()
}

type UserStore interface {
	GetUserByUsername(string username);
}