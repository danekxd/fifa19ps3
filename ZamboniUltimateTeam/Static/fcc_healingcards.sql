--
-- PostgreSQL database dump
--

\restrict 8MdxAA0qiwRBUvrymtPyd7J5l2kBHQkRYA7yU83x3vKNQugHq4nXLHUFaZLM8Wq

-- Dumped from database version 18.3
-- Dumped by pg_dump version 18.3

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET transaction_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- Name: fcc_healingcards; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.fcc_healingcards (
    carddbid integer,
    cardsubtype integer,
    weightrare integer,
    amount integer,
    zcat integer
);


ALTER TABLE public.fcc_healingcards OWNER TO postgres;

--
-- Data for Name: fcc_healingcards; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.fcc_healingcards (carddbid, cardsubtype, weightrare, amount, zcat) FROM stdin;
5002001	211	0	1	1
5002002	211	0	2	2
5002003	211	50	4	3
5002004	212	0	1	1
5002005	212	0	2	2
5002006	212	50	4	3
5002007	213	0	1	1
5002008	213	0	2	2
5002009	213	50	4	3
5002012	214	15	4	4
5002011	214	75	2	3
5002010	214	100	1	2
\.


--
-- PostgreSQL database dump complete
--

\unrestrict 8MdxAA0qiwRBUvrymtPyd7J5l2kBHQkRYA7yU83x3vKNQugHq4nXLHUFaZLM8Wq

